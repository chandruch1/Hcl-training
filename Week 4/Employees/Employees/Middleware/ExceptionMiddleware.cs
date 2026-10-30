using System.Text.Json;

namespace Employees.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        public ExceptionMiddleware(RequestDelegate next,ILogger<ExceptionMiddleware> logger)
        {
            _logger = logger;
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch(Exception e)
            {
                _logger.LogError(e,"An unexpected error occurred.");
                context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                context.Response.ContentType = "application/problem+json";
                var response= new
                {
                    type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                    title = "Internal Server Error",
                    status = 500,
                    detail = "An unexpected error occurred."
                };
                await context.Response.WriteAsJsonAsync(response);

            }
        }
    }
}
