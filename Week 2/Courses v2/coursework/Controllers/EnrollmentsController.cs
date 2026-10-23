using CoursesWebApp.Models;
using CoursesWebApp.Services;
using Microsoft.AspNetCore.Mvc;

namespace CoursesWebApp.Controllers;

[ApiController]
[Route("api/enrollments")]
public class EnrollmentsController(EnrollmentService enrollments) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll() => Ok(await enrollments.GetAllAsync());

    [HttpPost]
    public async Task<IActionResult> Add(CreateEnrollmentRequest request)
    {
        try { return Ok(await enrollments.AddAsync(request.LearnerId, request.CourseCode)); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Cancel(int id)
    {
        try { await enrollments.CancelAsync(id); return Ok(new { message = "Enrollment cancelled." }); }
        catch (KeyNotFoundException ex) { return NotFound(new { message = ex.Message }); }
        catch (InvalidOperationException ex) { return BadRequest(new { message = ex.Message }); }
    }
}
