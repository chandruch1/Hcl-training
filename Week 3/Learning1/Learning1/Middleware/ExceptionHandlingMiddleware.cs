
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Learning1.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "An unexpected error occurred.");

                if (context.Response.HasStarted)
                {
                    _logger.LogWarning(
                        "Response has already started. Rethrowing exception.");

                    throw;
                }

                _logger.LogInformation(
                    "Response has not started. Sending 500 error response.");

                context.Response.Clear();

                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                context.Response.ContentType = "application/json";

                var response = new
                {
                     statusCode = 500,
                    message = "An unexpected server error occurred."
                };

                await context.Response.WriteAsync(
                    JsonSerializer.Serialize(response));
            }

            _logger.LogInformation("Middleware finished.");
        }
    }
}
