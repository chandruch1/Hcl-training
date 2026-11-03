using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieTicketBooking.Data;
using MovieTicketBooking.DTOs.Auth;
using MovieTicketBooking.Models;

namespace MovieTicketBooking.Services
{
    public class Authservice
    {
        private readonly ApplicationDbContext context;
        public Authservice(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<User?> ValidateUser(LoginRequestDto dto)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if(user is null)
            {
                return null;
            }
            var Ispasswordvalid = BCrypt.Net.BCrypt.Verify(dto.PassWord, user.HashPassword);
            if (!Ispasswordvalid)
            {
                return null;
            }
            return user;
        }

        public async Task<User?> RegisterAsync(RegisterRequestDto dto)
        {
            var Exsist = await context.Users.AnyAsync(u => u.Email == dto.Email);
            if (Exsist)
            {
                return null;
            }
            var user = new User
            {
                Email = dto.Email,
                UserName = dto.UserName,
                HashPassword = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                IsActive=true,
                role = Role.Customer

            };

            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;

        }

       
    }
}
