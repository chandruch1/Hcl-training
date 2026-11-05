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
            if (context.Request.Path.StartsWithSegments("api/admin"))
            {
                if (context.User.Identity?.IsAuthenticated != true)
                {
                    context.Response.StatusCode = 401;

                    await context.Response.WriteAsync(
                        "You are not authenticated"
                    );
                    return;
                }
            }
            if (!context.User.IsInRole("ADMIN"))
            {
                context.Response.StatusCode = 403;

                await context.Response.WriteAsync(
                    "You don't have permission to access this resource"
                );

                return;
            }

            try
            {
                await _next(context);
            }
            catch(Exception )
            {
                Console.WriteLine("Admin please authenticate");
            }
        }
    }
}
