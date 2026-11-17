using Employees.DTO;
using Employees.Services;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AuthService _authService;
        private readonly JwtService _jwtService;
        private readonly RefreshTokenService _refreshTokenService;

        public AuthController(AuthService authService, JwtService jwtService, RefreshTokenService refreshTokenService)
        {
            _authService = authService;
            _jwtService = jwtService;
            _refreshTokenService = refreshTokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var user = await _authService.ValidateUserAsync(dto);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid email or password"
                });
            }

            var token = _jwtService.GenerateAccessToken(user);
            var refreshToken = _refreshTokenService.GenerateRefreshToken();

            var tokenHash = _refreshTokenService.HashRefreshToken(refreshToken);


            await _authService.SaveRefreshTokenAsync(user.UserId, tokenHash);

            return Ok(new
            {
                message = "Login successful",
                accessToken = token,
                refreshToken = refreshToken,
                userId = user.UserId,
                email = user.Email,
                role = user.Role.ToString()
            });
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            var user = await _authService.RegisterAsync(dto);

            if (user == null)
            {
                return Conflict(new
                {
                    message = "Email already exists"
                });
            }

            return StatusCode(200, new
            {
                message = "Registration successful",
                userId = user.UserId,
                email = user.Email,
                role = user.Role.ToString()
            });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(RefreshTokenRequestDto dto)
        {
            // 1. Validate old refresh token
            var user = await _authService.ValidateRefreshTokenAsync(dto.RefreshToken);

            if (user == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid or expired refresh token"
                });
            }

            // 2. Generate new access token
            var accessToken = _jwtService.GenerateAccessToken(user);

            // 3. Generate new refresh token
            var newRefreshToken = _refreshTokenService.GenerateRefreshToken();

            // 4. Hash new refresh token
            var tokenHash = _refreshTokenService.HashRefreshToken(newRefreshToken);

            // 5. Revoke old refresh token
            var revoked = await _authService.RevokeRefreshTokenAsync(dto.RefreshToken);

            if (!revoked)
            {
                return Unauthorized(new
                {
                    message = "Refresh token is no longer valid"
                });
            }

            // 6. Save new refresh token
            await _authService.SaveRefreshTokenAsync(user.UserId, tokenHash);

            return Ok(new
            {
                message = "Token refreshed successfully",
                accessToken = accessToken,
                refreshToken = newRefreshToken
            });
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout(RefreshTokenRequestDto dto)
        {
            var revoked = await _authService.RevokeRefreshTokenAsync(dto.RefreshToken);

            if (!revoked)
            {
                return Unauthorized(new
                {
                    message = "Invalid or already revoked refresh token"
                });
            }

            return Ok(new
            {
                message = "Logout successful"
            });
}
    }
}