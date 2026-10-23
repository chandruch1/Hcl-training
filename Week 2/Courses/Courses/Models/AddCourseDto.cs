
namespace CoursesWebApi.Models;

public class AddCourseDto
{
    public string CourseCode { get; set; } = "";
    public string Title { get; set; } = "";
    public decimal Fee { get; set; }
    public int DurationDays { get; set; }
    public int TotalSeats { get; set; }
}
