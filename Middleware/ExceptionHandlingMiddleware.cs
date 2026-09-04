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
                var (statusCode, message) = ex switch
                {
                    NotFoundException => (HttpStatusCode.NotFound, ex.Message),
                    BusinessRuleException => (HttpStatusCode.BadRequest, ex.Message),
                    InvalidOperationException => (HttpStatusCode.BadRequest, ex.Message),
                    _ => (HttpStatusCode.InternalServerError, "Ocurrió un error inesperado.")
                };

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)statusCode;

                var result = JsonSerializer.Serialize(new { error = message });
                await context.Response.WriteAsync(result);
            }
        }
    }
}