
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Todo.DataAccessLayer.Data;
using Todo.DataAccessLayer.Interface;
using Todo.DataAccessLayer.Repository;
using Todo.ServiceLayer.Interface;
using Todo.ServiceLayer.Services;

namespace Todo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();


            builder.Services.AddScoped<ITodoService, TodoService>();
            builder.Services.AddScoped<ITodoRepositoy, TodoRepository>();

            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();

            builder.Services.AddScoped<IValidationsTodo, ValidationsTodo>();

            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<ILoginRepo, LoginRepo>();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            //builder.Services.AddSwaggerGen();

            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Todo API",
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



            // Register DbContext with connection string
            builder.Services.AddDbContext<TodoContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("MyDb")));


            // JWT =================================================================================

            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = builder.Configuration["JWTConfiguration:Issuer"],
                    ValidAudience = builder.Configuration["JWTConfiguration:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWTConfiguration:Key"]))
                };
            });

            // ==========================================================================================

            var app = builder.Build();

            app.UseMiddleware<ExceptionMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            //========================
            app.UseAuthentication();
            //========================
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
