using System.ComponentModel.DataAnnotations.Schema;

namespace CoursesWebApp.Models;

public class Course
{
    public string CourseCode { get; set; } = "";
    public string Title { get; set; } = "";
    public decimal Fee { get; set; }
    public int DurationDays { get; set; }
    public int TotalSeats { get; set; }
    public int EnrolledCount { get; set; }

    [NotMapped]
    public int AvailableSeats => TotalSeats - EnrolledCount;
}
