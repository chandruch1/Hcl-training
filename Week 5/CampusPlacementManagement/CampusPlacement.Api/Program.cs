using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using CampusPlacement.Api.Data;
using CampusPlacement.Api.Models;

var builder = WebApplication.CreateBuilder(args);

// 1. Database configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    )
);

// 2. ASP.NET Core Identity configuration
builder.Services
    .AddIdentity<ApplicationUser, IdentityRole>(options =>
    {
        // Password rules
        options.Password.RequiredLength = 8;
        options.Password.RequireDigit = true;
        options.Password.RequireUppercase = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = true;

        // User rules
        options.User.RequireUniqueEmail = true;

        // Lockout rules
        options.Lockout.MaxFailedAccessAttempts = 5;
        options.Lockout.DefaultLockoutTimeSpan =
            TimeSpan.FromMinutes(15);
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

// 3. Controller configuration
builder.Services.AddControllers();

builder.Services.AddOpenApi();

var app = builder.Build();

// 4. Middleware configuration
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();