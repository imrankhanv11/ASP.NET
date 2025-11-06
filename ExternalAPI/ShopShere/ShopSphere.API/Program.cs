using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ShopSphere.API.GlobalExceptionHandler;
using ShopSphere.DataAccessLayer.Data;
using ShopSphere.DataAccessLayer.Interface;
using ShopSphere.DataAccessLayer.Repository;
using ShopSphere.ServiceLayer.Interface;
using ShopSphere.ServiceLayer.Services;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;

namespace ShopSphere.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Logging Configuratons
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            //External API Setup
            builder.Services.AddHttpClient<IExternalAPIService, ExternalAPIService>(client =>
            {
                client.BaseAddress = new Uri(builder.Configuration["ReceiverAPI:EndPoint"]);
            });

            //builder.Services.AddControllers();

            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
            });

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy => policy
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // Swagger Authentication Configuration
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Shop_Sphere",
                    Version = "v1"
                });

                // Add JWT Security Definition
                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header
                });

                // Apply JWT globally
                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            // DB Context Configuration
            builder.Services.AddDbContext<ShopSphereContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("MyDb"))
            );


            // Dependency Injections
            // Authentications
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IAthenticationRepo, AuthenticationRepo>();

            // Products
            builder.Services.AddScoped<IProductService, ProductSerive>();
            builder.Services.AddScoped<IProductRepo, ProductRepo>();

            // Carts
            builder.Services.AddScoped<ICartRepo, CartRepo>();
            builder.Services.AddScoped<ICartService, CartService>();

            // External API (not needed)
            //builder.Services.AddScoped<IExternalAPIService, ExternalAPIService>();

            // Adding Policy
            builder.Services.AddAuthorization(options =>
            {
                // Policy for User Access Only (for Cart end points)
                options.AddPolicy("UsersOnly", policy =>
                    policy.RequireRole("User")
                );

                // Policy for Admin Access Only (for Product Add end points)
                options.AddPolicy("AdminOnly", policy =>
                    policy.RequireRole("Admin")
                );

                // Policy for User and Admin Access (for product View end point)
                options.AddPolicy("AdminOrUser", policy =>
                    policy.RequireRole("Admin","User")
                );
            });

            //JWT Token Configuration
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = true,
                        ValidateIssuer = true,
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidIssuer = builder.Configuration["JWTConnection:Issuer"],
                        ValidAudience = builder.Configuration["JWTConnection:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTConnection:Key"])),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            // Auto Mapper Configurations
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            var app = builder.Build();

            // GlobalException Middler ware
            app.UseMiddleware<ExceptionHandler>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowReactApp");     // <- first
            app.UseAuthentication();          // <- second
            app.UseAuthorization();           // <- third

            app.MapControllers();

            app.Run();
        }
    }
}
