using System.Net;
using System.Text.Json;
using ProductsApi.Domain.Exceptions;

namespace ProductsApi.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var (statusCode, errorType, message) = ex switch
                {
                    NotFoundException => (HttpStatusCode.NotFound, "NotFound", ex.Message),
                    BusinessRuleException => (HttpStatusCode.BadRequest, "BusinessRuleError", ex.Message),
                    InvalidOperationException => (HttpStatusCode.BadRequest, "InvalidOperation", ex.Message),
                    _ => (HttpStatusCode.InternalServerError, "InternalServerError", "Ocurrió un error inesperado.")
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;

                var response = new 
                { 
                    error = errorType, 
                    message = message 
                };

                var result = JsonSerializer.Serialize(response);
                await context.Response.WriteAsync(result);
            }
        }
    }
}