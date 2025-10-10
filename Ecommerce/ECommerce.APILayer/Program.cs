using ECommerce.APILayer.GlobalExceptionHandler;
using ECommerce.DataAccessLayer.Data;
using ECommerce.DataAccessLayer.Inteface;
using ECommerce.DataAccessLayer.Repository;
using ECommerce.ServiceLayer.Interface;
using ECommerce.ServiceLayer.Service;
//using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
//using ECommerce.ServiceLayer.FluentValidations;

namespace ECommerce.APILayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // configure logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();


            // Fluent Validations
            //builder.Services.AddFluentValidation();
            //builder.Services.AddValidatorsFromAssemblyContaining<RegisterUserDTOValidations>();


            // Dependency Injection
            builder.Services.AddScoped<IAuthRepo, AuthRepo>();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<ICustomerService, CustomerService>();
            builder.Services.AddScoped<ICustomerRepo, CustomerRepo>();
            builder.Services.AddScoped<IProductService, ProductService>();

            // Generic Repo Injection
            builder.Services.AddScoped(typeof(IGenericRepo<>), typeof(GenericRepo<>));


            // Auto Mapper
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            // Swagger Authentication
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Book Mangement",
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

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAny_SPAdmin_Or_Admin", policy =>
                    policy.RequireRole("SPAdmin", "Admin")
                );

                options.AddPolicy("UsersOnly", policy =>
                    policy.RequireRole("User")
                );
            });

            // DB Context Configuration
            builder.Services.AddDbContext<ECommerceContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("MyDb"))
            );

            // JWT Token
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
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTConnection:Key"]))
                };
            });


            var app = builder.Build();

            // Global Exceptions Handler
            app.UseMiddleware<ExceptionHandler>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            // JWT Token
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
