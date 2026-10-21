using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Models;
using StudentApi.Services;

namespace StudentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController(AuthService authService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("login")]
    public IActionResult Login(LoginRequest request)
    {
        var response = authService.Login(request);

        if (response == null)
        {
            return Unauthorized(new
            {
                message = "Invalid username or password"
            });
        }

        return Ok(response);
    }
}