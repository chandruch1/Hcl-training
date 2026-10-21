using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using StudentApi.Services;

namespace StudentApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController(StudentService studentService)
    : ControllerBase
{
    // Any authenticated user can access this endpoint.
    [Authorize]
    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(studentService.GetAll());
    }

    // Any authenticated user can access a student by ID.
    [Authorize]
    [HttpGet("{id:int}")]
    public IActionResult GetById(int id)
    {
        var student = studentService.GetById(id);

        if (student == null)
            return NotFound(new { message = "Student not found" });

        return Ok(student);
    }

    // Only Admin can access this endpoint.
    [Authorize(Roles = "Admin")]
    [HttpGet("admin")]
    public IActionResult AdminOnly()
    {
        return Ok(new
        {
            message = "Welcome Admin. Access granted."
        });
    }

    // Only users satisfying StudentAccess can access this.
    [Authorize(Policy = "StudentAccess")]
    [HttpGet("student-area")]
    public IActionResult StudentArea()
    {
        return Ok(new
        {
            message = "Student policy authorization successful."
        });
    }

    // View the identity and role from the validated token.
    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        return Ok(new
        {
            Username = User.Identity?.Name,
            Role = User.FindFirst(
                System.Security.Claims.ClaimTypes.Role)?.Value
        });
    }
}