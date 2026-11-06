using CampusPlacement.Api.DTOs;
using CampusPlacement.Api.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusPlacement.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class CollegeController : ControllerBase
{
    private readonly ICollegeService _collegeService;

    public CollegeController(ICollegeService collegeService)
    {
        _collegeService = collegeService;
    }

    // Register College
    [HttpPost("register")]
    [AllowAnonymous]
    public async Task<IActionResult> Register(
        CollegeRegistrationRequest request)
    {
        var college = await _collegeService.RegisterAsync(request);

        if (college == null)
        {
            return Conflict(new
            {
                message = "College code already exists."
            });
        }

        return CreatedAtAction(
            nameof(GetById),
            new { id = college.CollegeId },
            college
        );
    }

    // Get All Colleges
    [HttpGet]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetAll()
    {
        var colleges = await _collegeService.GetAllAsync();

        return Ok(colleges);
    }

    // Get College By ID
    [HttpGet("{id}")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetById(int id)
    {
        var college = await _collegeService.GetByIdAsync(id);

        if (college == null)
        {
            return NotFound(new
            {
                message = "College not found."
            });
        }

        return Ok(college);
    }

    // Get Pending Colleges
    [HttpGet("pending")]
    [Authorize(Roles = "SuperAdmin")]
    public async Task<IActionResult> GetPending()
    {
        var colleges = await _collegeService.GetPendingAsync();

        return Ok(colleges);
    }
}