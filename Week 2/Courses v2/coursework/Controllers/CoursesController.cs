using CoursesWebApp.Models;
using CoursesWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoursesWebApp.Controllers;

[ApiController]
[Route("api/courses")]
public class CoursesController(CourseService courses) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await courses.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Add(AddCourseRequest request)
    {
        try { return Ok(await courses.AddAsync(request)); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpDelete("{code}")]
    public async Task<IActionResult> Delete(string code)
    {
        try { await courses.DeleteAsync(code); return NoContent(); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }
}
