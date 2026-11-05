using Microsoft.AspNetCore.Identity;
using CampusPlacement.Api.Models;
using CampusPlacement.Api.Enums;
namespace CampusPlacement.Api.Data;

public static class IdentitySeeder
{
    public static async Task SeedRolesAsync(
        IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<IdentityRole>>();

        string[] roles =
        {
            "SuperAdmin",
            "PlacementOfficer",
            "CompanyHR",
            "Student"
        };

        foreach (var role in roles)
        {
            bool roleExists =
                await roleManager.RoleExistsAsync(role);

            if (!roleExists)
            {
                var result = await roleManager.CreateAsync(
                    new IdentityRole(role));

                if (!result.Succeeded)
                {
                    throw new Exception(
                        $"Failed to create role: {role}. " +
                        string.Join(", ",
                            result.Errors.Select(e => e.Description))
                    );
                }

                Console.WriteLine($"Role created: {role}");
            }
            else
            {
                Console.WriteLine($"Role already exists: {role}");
            }
        }
    }
    public static async Task SeedSuperAdminAsync(
    IServiceProvider serviceProvider,
    IConfiguration configuration)
{
    using var scope = serviceProvider.CreateScope();

    var userManager = scope.ServiceProvider
        .GetRequiredService<UserManager<ApplicationUser>>();

    // Read credentials from User Secrets
    string? email = configuration["InitialAdmin:Email"];

    string? password = configuration["InitialAdmin:Password"];

    // Check configuration
    if (string.IsNullOrWhiteSpace(email) ||
        string.IsNullOrWhiteSpace(password))
    {
        throw new Exception("SuperAdmin credentials are missing.");
    }

    // Check whether SuperAdmin already exists
    var existingUser = await userManager.FindByEmailAsync(email);

    if (existingUser == null)
    {
        // Create SuperAdmin user object
        var admin = new ApplicationUser
        {
            UserName = email,
            Email = email,
            FullName = "System Administrator",
            AccountStatus = AccountStatus.Approved,
            CreatedAt = DateTime.UtcNow,
            EmailConfirmed = true
        };

        // Create user and hash password
        var result = await userManager.CreateAsync(admin, password);

        if (!result.Succeeded)
        {
            throw new Exception(
                "SuperAdmin creation failed: " +
                string.Join(", ",
                    result.Errors.Select(e => e.Description))
            );
        }

        Console.WriteLine("SuperAdmin account created.");
        existingUser = admin;
    }
    else
    {
        Console.WriteLine("SuperAdmin account already exists.");
    }

    // Assign SuperAdmin role if not already assigned
    if (!await userManager.IsInRoleAsync(existingUser, "SuperAdmin"))
    {
        var roleResult = await userManager.AddToRoleAsync(
            existingUser, "SuperAdmin");

        if (!roleResult.Succeeded)
        {
            throw new Exception(
                "SuperAdmin role assignment failed: " +
                string.Join(", ",
                    roleResult.Errors.Select(e => e.Description))
            );
        }

        Console.WriteLine("SuperAdmin role assigned.");
    }
}
}