using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;

var builder = WebApplication.CreateBuilder(args);

// ─────────────────────────────────────────────────────────────────
// 1. CONTROLLERS
//    AddControllers() registers all classes decorated with [ApiController]
//    into the DI container and sets up the MVC pipeline.
// ─────────────────────────────────────────────────────────────────
builder.Services.AddControllers();

// ─────────────────────────────────────────────────────────────────
// 2. DATABASE — Entity Framework Core + SQL Server
//
//    GetConnectionString("DefaultConnection") reads from appsettings.json.
//    We never hardcode the connection string here.
//
//    UseSqlServer() registers the SQL Server database provider.
//    ApplicationDbContext is registered as a Scoped service automatically
//    by AddDbContext — one DbContext instance per HTTP request.
// ─────────────────────────────────────────────────────────────────
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// ─────────────────────────────────────────────────────────────────
// 3. SWAGGER / OPENAPI
//
//    AddEndpointsApiExplorer() — required for minimal-API endpoint discovery
//    AddSwaggerGen()            — generates the OpenAPI JSON spec
// ─────────────────────────────────────────────────────────────────
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "Movie Ticket Booking API",
        Version = "v1",
        Description = "ASP.NET Core Web API for managing movie ticket bookings. " +
                      "Built with .NET 10, EF Core 10, and SQL Server."
    });
});

// ─────────────────────────────────────────────────────────────────
// 4. CORS (pre-configured for future Angular frontend integration)
//    Policy name "AllowAngular" will be referenced in Phase 11.
// ─────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
    {
        policy.WithOrigins("http://localhost:4200")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// ─────────────────────────────────────────────────────────────────
// Services and Repositories will be registered here in later phases.
// Example (Phase 5 onwards):
//   builder.Services.AddScoped<IMovieRepository, MovieRepository>();
//   builder.Services.AddScoped<IMovieService, MovieService>();
// ─────────────────────────────────────────────────────────────────

var app = builder.Build();

// ─────────────────────────────────────────────────────────────────
// MIDDLEWARE PIPELINE
// Order matters: exception handling → HTTPS → routing → CORS →
//                auth/authz → controllers
// ─────────────────────────────────────────────────────────────────

// Global exception handling middleware will be added in Phase 9.

if (app.Environment.IsDevelopment())
{
    // Expose Swagger UI only in Development for security.
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Movie Ticket Booking API v1");
        options.RoutePrefix = string.Empty; // Swagger UI available at root: https://localhost:{port}/
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAngular");

// MapControllers() activates attribute-based routing.
// Every [Route] and [HttpGet/Post/Put/Delete] attribute on controllers
// is registered here.
app.MapControllers();

app.Run();
