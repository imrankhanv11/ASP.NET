using System.ComponentModel.DataAnnotations;
using System.Net;
 
namespace BookManagement.API
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
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
                await HandleExceptionAsync(context, (int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                await HandleExceptionAsync(context, (int)HttpStatusCode.NotFound, ex.Message);
            }
            catch (ValidationException ex)
            {
                await HandleExceptionAsync(context, (int)HttpStatusCode.BadRequest, ex.Message);
            }
            catch(UnauthorizedAccessException ex)
            {
                await HandleExceptionAsync(context, (int)HttpStatusCode.Unauthorized, ex.Message);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, (int)HttpStatusCode.InternalServerError, "Internal Server Error: " + ex.Message);
            }
        }

        private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
        {
            context.Response.StatusCode = statusCode;

            context.Response.ContentType = "application/json";

            var result = new
            {
                statusCode,
                message
            };

            await context.Response.WriteAsJsonAsync(result);
        }
    }
    }