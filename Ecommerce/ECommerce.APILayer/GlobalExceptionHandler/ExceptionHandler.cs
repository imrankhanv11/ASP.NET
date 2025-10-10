using System.ComponentModel.DataAnnotations;
using System.Net;

namespace ECommerce.APILayer.GlobalExceptionHandler
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
            catch(ArgumentException ex)
            {
                await HandleExceptions(context, (int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptions(context, (int)HttpStatusCode.NotFound, ex.Message);
            }
            catch (ValidationException ex)
            {
                await HandleExceptions(context, (int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                await HandleExceptions(context, (int)HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (Exception ex)
            {
                await HandleExceptions(context, (int)HttpStatusCode.InternalServerError, "Internal Server Error: " + ex.Message);
            }
        }

        private static async Task HandleExceptions(HttpContext context, int statuscode, string message)
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
