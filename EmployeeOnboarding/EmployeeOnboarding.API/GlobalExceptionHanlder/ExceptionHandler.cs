using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Reflection;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EmployeeOnboarding.API.GlobalExceptionHanlder
{
    public class ExceptionHandler
    {
        private readonly RequestDelegate _next;

        public ExceptionHandler(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (ArgumentException ex)
            {
                await ExceptionMethod(context, (int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                await ExceptionMethod(context, (int)HttpStatusCode.NotFound, ex.Message);
            }
            catch (ValidationException ex)
            {
                await ExceptionMethod(context, (int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (Exception ex)
            {
                await ExceptionMethod(context, (int)HttpStatusCode.InternalServerError, "Internal Server Error: " + ex.Message);
            }
        }

        private static async Task ExceptionMethod(HttpContext context, int statuscode, string message)
        {
            context.Response.StatusCode = statuscode;

            context.Response.ContentType = "application/json";

            var result = new
            {
                statuscode,
                message
            };

            await context.Response.WriteAsJsonAsync(result);
        }
    }
}
