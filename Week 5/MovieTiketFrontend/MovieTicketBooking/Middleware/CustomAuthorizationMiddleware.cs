namespace MovieTicketBooking.Middleware
{
    public class CustomAuthorizationMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomAuthorizationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            
            if (context.Request.Path.StartsWithSegments("/api/admin"))
            {
                // Check authentication
                if (context.User.Identity?.IsAuthenticated != true)
                {
                    context.Response.StatusCode = 401;

                    await context.Response.WriteAsync(
                        "You are not authenticated"
                    );

                    return;
                }

                // Check role
                if (!context.User.IsInRole("ADMIN"))
                {
                    context.Response.StatusCode = 403;

                    await context.Response.WriteAsync(
                        "You don't have permission to access this resource"
                    );

                    return;
                }
            }

            // Allow request to continue
            await _next(context);
        }
    }
}