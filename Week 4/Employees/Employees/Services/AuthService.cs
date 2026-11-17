
using Employees.Data;
using Employees.DTO;
using Employees.Enums;
using Employees.Models;
using Microsoft.EntityFrameworkCore;


namespace Employees.Services
{
    public class AuthService
    {
        private readonly AppDbContext _context;
        private readonly RefreshTokenService _refreshTokenService;

        public AuthService(
                 AppDbContext context,
                 RefreshTokenService refreshTokenService)
        {
            _context = context;
            _refreshTokenService = refreshTokenService;
        }

        public async Task<User?> ValidateUserAsync(LoginRequestDto dto)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == dto.Email);

            if (user == null || !user.IsActive)
            {
                return null;
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(
                dto.Password,
                user.PasswordHash
            );

            if (!isPasswordValid)
            {
                return null;
            }

            return user;
        }

        public async Task<User?> RegisterAsync(RegisterRequestDto dto)
        {
            var existingUser = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (existingUser)
            {
                return null;
            }
            var user = new User {
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = dto.Role,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return user;

        }

        public async Task SaveRefreshTokenAsync(int userId, string tokenHash)
        {
            var refreshToken = new RefreshToken
            {
                UserId = userId,
                TokenHash = tokenHash,
                CreatedAt = DateTime.UtcNow,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            };

            _context.RefreshTokens.Add(refreshToken);

            await _context.SaveChangesAsync();
        }

        public async Task<User?> ValidateRefreshTokenAsync(string token)
        {
            var tokenHash = _refreshTokenService.HashRefreshToken(token); 

            var refreshToken = await _context.RefreshTokens
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.TokenHash == tokenHash);

            if (refreshToken == null ||
                refreshToken.RevokedAt != null ||
                refreshToken.ExpiresAt <= DateTime.UtcNow ||
                refreshToken.User == null ||
                !refreshToken.User.IsActive)
            {
                return null;
            }

            return refreshToken.User;
        }

        public async Task<bool> RevokeRefreshTokenAsync(string token)
        {
            var tokenHash = _refreshTokenService.HashRefreshToken(token);

            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(r => r.TokenHash == tokenHash);

            if (refreshToken == null ||
                refreshToken.RevokedAt != null ||
                refreshToken.ExpiresAt <= DateTime.UtcNow)
            {
                return false;
            }

            refreshToken.RevokedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return true;
        }
    }
}
