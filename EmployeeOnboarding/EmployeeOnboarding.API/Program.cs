using EmployeeOnboarding.API.GlobalExceptionHanlder;
using EmployeeOnboarding.DataAccessLayer.Data;
using EmployeeOnboarding.DataAccessLayer.Interfaces;
using EmployeeOnboarding.DataAccessLayer.Repositories;
using EmployeeOnboarding.ServiceLayer.Helper.HelperService;
using EmployeeOnboarding.ServiceLayer.Helper.Interface;
using EmployeeOnboarding.ServiceLayer.Interfaces;
using EmployeeOnboarding.ServiceLayer.Services;
using EmployeeOnboarding.ServiceLayer.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace EmployeeOnboarding.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Logg Configurations
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddValidatorsFromAssemblyContaining<EmployeeOnboardRequestDTOValidator>();

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowReactApp",
                    policy => policy
                    .WithOrigins("http://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials());
            });

            // Dependecy Injections
            // Employee
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();

            // MetaLog
            builder.Services.AddScoped<IMetaLogRepository, MetaLogRepository>();
            builder.Services.AddScoped<IMetaLogService, MetaLogService>();

            // HelperService
            builder.Services.AddScoped<IHelperService, HelperService>();

            // Hod
            builder.Services.AddScoped<IHodRepositroy, HodRepository>();

            //Department
            builder.Services.AddScoped<IDepartmentRepo, DepartmentRepo>();
            builder.Services.AddScoped<IDepartmentService, DepartmentService>();

            // DbContext Configurations
            builder.Services.AddDbContext<EmployeeContext>(option =>
               option.UseSqlServer(builder.Configuration.GetConnectionString("MyDb")));

            var app = builder.Build();


            // GlobalException Middlerware
            app.UseMiddleware<ExceptionHandler>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();


            app.UseCors("AllowReactApp");
            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
