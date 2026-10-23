
using Courses.Models;
using Courses.Services;
using CoursesWebApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoursesWebApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CourseController : ControllerBase
{
    private readonly CourseService _courseService;

    public CourseController(CourseService courseService)
    {
        _courseService = courseService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_courseService.GetAll());
    }

    [HttpPost]
    public IActionResult Add(AddCourseDto dto)
    {
        try
        {
            var course = new Course(
                dto.CourseCode,
                dto.Title,
                dto.Fee,
                dto.DurationDays,
                dto.TotalSeats,
                0);

            _courseService.AddCourse(course);

            return Created(
                $"/api/course/{course.CourseCode}",
                course);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
