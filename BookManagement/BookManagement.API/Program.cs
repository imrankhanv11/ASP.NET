using BookManagement.DataAccessLayer.Data;
using BookManagement.DataAccessLayer.Repositories;
using BookManagement.ServiceLayer.Services;
using BookManagement.DataAccessLayer.Interfaces;
using BookManagement.ServiceLayer.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.WebSockets;

namespace BookManagement.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy => policy
                    .WithOrigins("http://127.0.0.1:5500") // your AngularJS frontend
                    .AllowAnyHeader()
                    .AllowAnyMethod());
            });

            // configure logging
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Depentency Injections
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<ILoginRepo, LoginRepo>();

            builder.Services.AddScoped<IProductServices, ProductServices>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<ISPAdminService, SPAdminService>();

            builder.Services.AddScoped<IProductRepo, ProductRepo>();

            builder.Services.AddScoped<ICatRepo, CatRepo>();

            //// Testing config
            builder.Services.AddScoped<ITestingService, TestingService>();

            // Generic Repository
            builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

            // Mapper Injecton
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());


            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

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


            // Connection DBContext
            builder.Services.AddDbContext<BookContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("MyDb")));

            // giving Policy
            builder.Services.AddAuthorization(options => {
                options.AddPolicy("SPAdminOnly", policy =>
                    policy.RequireRole("SPAdmin")
                );

                options.AddPolicy("SPAdminAndAdmin", policy =>
                    policy.RequireRole("SPAdmin", "Admin")
                );
            });

            // JWT
            builder.Services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWTConnection:Issuer"],
                    ValidAudience = builder.Configuration["JWTConnection:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTConnection:Key"])),
                    ClockSkew = TimeSpan.Zero
                };
            });


            var app = builder.Build();

            // minimal api
            app.MapGet("/Book", async (IProductServices service) =>
            {
                var cat = await service.GetAllBooksService();

                return Results.Ok(cat);
            });

            // Exception Middleware
            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors("AllowReactApp");

            // Authentication for JWT
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
