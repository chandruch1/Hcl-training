using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.DTOs.Admin;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services
{
    public class AdminService
    {
        private readonly ApplicationDbContext _context;
        public AdminService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User?> RegisterAsync(AdminRegisterDto dto)
        {
            var user = await _context.Users.AnyAsync(u => u.Email == dto.Email);
            if (user)
            {
                return null;
            }
            var admin = new User { 
                UserName=dto.UserName,
                Email = dto.Email,
                HashPassword=BCrypt.Net.BCrypt.HashPassword(dto.Password),
                role= Role.Admin,
                IsActive=true,
                CreatedAt=DateTime.UtcNow
            };

            _context.Users.Add(admin);
            await _context.SaveChangesAsync();

            return admin;

        }

        public async Task<User?> LoginAdminAsync(LoginRequestDto dto)
        {
            var admin = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if(admin==null || !admin.IsActive)
            {
                return null;
            }
            var Isvalid = BCrypt.Net.BCrypt.Verify(dto.passWord, admin.HashPassword);
            if (!Isvalid)
            {
                return null;
            }
            return admin;
        }
    }

}
