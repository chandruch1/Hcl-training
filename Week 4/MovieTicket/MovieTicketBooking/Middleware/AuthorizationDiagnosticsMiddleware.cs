
using Microsoft.AspNetCore.Http;

public class AuthorizationDiagnosticsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthorizationDiagnosticsMiddleware> _logger;

    public AuthorizationDiagnosticsMiddleware(
        RequestDelegate next,
        ILogger<AuthorizationDiagnosticsMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation(
            "========== AUTHORIZATION PROCESS ==========");

        await _next(context);

        // Executes after the remaining pipeline
        _logger.LogInformation(
            "Final Response Status: {StatusCode}",
            context.Response.StatusCode);
    }
}
