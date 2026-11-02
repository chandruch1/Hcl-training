using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using MovieTicketBooking.Models;
using Microsoft.IdentityModel.Tokens;

namespace MovieTicketBooking.Services
{
    public class JwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string GenerateToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var Claims = new List<Claim> { 
                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                new Claim (ClaimTypes.Email,user.Email),
                new Claim (ClaimTypes.Role,user.role.ToString())
            };

            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["jwt:ExpiryMinutes"])),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler
        }
    }
}
