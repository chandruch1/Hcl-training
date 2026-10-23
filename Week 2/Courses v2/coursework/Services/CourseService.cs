using CoursesWebApp.Data;
using CoursesWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CoursesWebApp.Services;

public class CourseService(ApplicationDbContext db)
{
    public async Task<List<Course>> GetAllAsync() => await db.Courses.AsNoTracking().OrderBy(c => c.CourseCode).ToListAsync();

    public async Task<Course?> FindByCodeAsync(string code) =>
        await db.Courses.FirstOrDefaultAsync(c => c.CourseCode == code);

    public async Task<Course> AddAsync(AddCourseRequest request)
    {
        var code = request.CourseCode.Trim();
        if (string.IsNullOrWhiteSpace(code)) throw new ArgumentException("Course code is required.");
        if (string.IsNullOrWhiteSpace(request.Title)) throw new ArgumentException("Course title is required.");
        if (request.Fee < 0) throw new ArgumentException("Fee cannot be negative.");
        if (request.DurationDays <= 0) throw new ArgumentException("Duration must be positive.");
        if (request.TotalSeats <= 0) throw new ArgumentException("Total seats must be positive.");
        if (await db.Courses.AnyAsync(c => c.CourseCode == code)) throw new ArgumentException($"Course code {code} already exists.");

        var course = new Course { CourseCode = code, Title = request.Title.Trim(), Fee = request.Fee, DurationDays = request.DurationDays, TotalSeats = request.TotalSeats };
        db.Courses.Add(course);
        await db.SaveChangesAsync();
        return course;
    }

    public async Task DeleteAsync(string code)
    {
        var course = await db.Courses.FirstOrDefaultAsync(c => c.CourseCode == code)
            ?? throw new KeyNotFoundException("Course not found.");
        if (await db.Enrollments.AnyAsync(e => e.CourseCode == code))
            throw new InvalidOperationException("Cannot delete a course that has enrollment history.");
        db.Courses.Remove(course);
        await db.SaveChangesAsync();
    }
}
