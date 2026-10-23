using CoursesWebApp.Models;
using CoursesWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoursesWebApp.Controllers;

[ApiController]
[Route("api/learners")]
public class LearnersController(LearnerService learners) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await learners.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<IActionResult> GetById(int id)
    {
        var learner = await learners.FindByIdAsync(id);
        return learner is null ? NotFound(new { message = "Learner not found." }) : Ok(learner);
    }

    [HttpPost]
    public async Task<IActionResult> Add(RegisterLearnerRequest request)
    {
        try { return Ok(await learners.AddAsync(request)); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }
}
