
using CampusPlacement.Api.DTOs;
using CampusPlacement.Api.Enums;
using CampusPlacement.Api.Interfaces.Services;
using CampusPlacement.Api.Models;
using Microsoft.AspNetCore.Identity;

namespace CampusPlacement.Api.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly JwtService _jwtService;

    public AuthService(
        UserManager<ApplicationUser> userManager,
        JwtService jwtService)
    {
        _userManager = userManager;
        _jwtService = jwtService;
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request)
    {
        // 1. Find user by email
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
            return null;

        // 2. Verify password
        bool passwordValid = await _userManager.CheckPasswordAsync(
            user, request.Password);

        if (!passwordValid)
            return null;

        // 3. Check account status
        if (user.AccountStatus != AccountStatus.Approved)
            return null;

        // 4. Get user roles
        var roles = await _userManager.GetRolesAsync(user);

        if (roles.Count != 1)
            return null;

        string role = roles[0];

        // 5. Generate JWT token
        string token = await _jwtService.GenerateTokenAsync(user);

        // 6. Set token expiry
        var expiresAt = DateTime.UtcNow.AddMinutes(60);

        // 7. Return login response
        return new LoginResponse
        {
            Token = token,
            Email = user.Email!,
            FullName = user.FullName,
            Role = role,
            ExpiresAt = expiresAt
        };
    }
}
