using Microsoft.AspNetCore.Mvc;
using MovieTicketBooking.DTOs.Auth;
using MovieTicketBooking.Services;
using MovieTicketBooking.Models;
using MovieTicketBooking.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;

namespace MovieTicketBooking.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly JwtService _service;
        private readonly Authservice authservice;
        public AuthController(JwtService service,Authservice authservice)
        {
            _service = service;
            this.authservice = authservice;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto dto)
        {
            var exsistinguser =await authservice.ValidateUser(dto);
            if (exsistinguser == null)
            {
                return Unauthorized(new { message = "Email or password is Invalid" });

            }
            var token = _service.GenerateToken(exsistinguser);
            return Ok(new
            {
                Message = " Login Successfully",
                accesToken = token,
                Username = exsistinguser.UserId,
                Role=exsistinguser.role
            });

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto dto)
        {
            var register = authservice.RegisterAsync(dto);
            if(register == null)
            {
                return Unauthorized(new { message=" you ALready registerd please Login" });

            }

            return StatusCode(200, new
            {
                message = "Registration successful",
            });

        }
    }
}
