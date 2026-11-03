using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Admin;
using MovieTicketBooking.Services;

namespace MovieTicketBooking.Controllers
{
    
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController
    {
        private readonly AdminService _adminService;
        private readonly JwtService _JwtService;
        public AdminController(AdminService adminService, JwtService jwtService)
        {
            _adminService = adminService;
            _JwtService = jwtService;
        }
        [HttpPost("Register")]
        public async Task<IActionResult> RegisterAdmin(AdminRegisterDto dto)
        {
            var admin =await _adminService.RegisterAsync(dto);
            if(admin == null)
            {
                return Conflict(new
                {
                    message = "Email already exists"
                });

            }
            return Created("", new
            {
                message = "Admin registered successfully",
                adminId = admin.UserId,
                name = admin.UserName,
                email = admin.Email,
                role = admin.role.ToString()
            });
        }

        [HttpPost("login")]
        public async Task<IActionResult> LoginAdmin(LoginRequestDto dto)
        {
            var admin =await _adminService.LoginAdminAsync(dto);
            if (admin == null)
            {
                return Unauthorized(new
                {
                    message = "Invalid admin email or password"
                });
            }
            var token = _JwtService.GenerateToken(admin);
            return Ok(new
            {
                message = "Admin login successful",
                accessToken=token,
                adminId = admin.UserId,
                name = admin.UserName,
                email = admin.Email,
                role = admin.role.ToString()
            });

        }
    }
}
