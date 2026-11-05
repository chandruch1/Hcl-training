
using Microsoft.AspNetCore.Http;

public class AuthenticationDiagnosticsMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<AuthenticationDiagnosticsMiddleware> _logger;

    public AuthenticationDiagnosticsMiddleware(
        RequestDelegate next,
        ILogger<AuthenticationDiagnosticsMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        _logger.LogInformation(
            "========== AUTHENTICATION RESULT ==========");

        // Check whether the user is authenticated
        _logger.LogInformation(
            "Is Authenticated: {IsAuthenticated}",
            context.User.Identity?.IsAuthenticated);

        // Display authentication type
        _logger.LogInformation(
            "Authentication Type: {AuthenticationType}",
            context.User.Identity?.AuthenticationType);

        // Display JWT claims
        foreach (var claim in context.User.Claims)
        {
            _logger.LogInformation(
                "Claim Type: {ClaimType}, Claim Value: {ClaimValue}",
                claim.Type,
                claim.Value);
        }

        await _next(context);
    }
}
