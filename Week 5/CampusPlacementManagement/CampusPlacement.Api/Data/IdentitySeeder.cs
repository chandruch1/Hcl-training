using Microsoft.AspNetCore.Identity;

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
}