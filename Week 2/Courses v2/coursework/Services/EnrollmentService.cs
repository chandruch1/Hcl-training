using CoursesWebApp.Data;
using CoursesWebApp.Models;
using Microsoft.EntityFrameworkCore;

namespace CoursesWebApp.Services;

public class EnrollmentService(ApplicationDbContext db)
{
    public async Task<List<Enrollment>> GetAllAsync() => await db.Enrollments.AsNoTracking().OrderByDescending(e => e.EnrollmentId).ToListAsync();

    public async Task<Enrollment> AddAsync(int learnerId, string courseCode)
    {
        var learnerExists = await db.Learners.AnyAsync(l => l.LearnerId == learnerId);
        if (!learnerExists) throw new KeyNotFoundException("Register the learner first.");
        var course = await db.Courses.FirstOrDefaultAsync(c => c.CourseCode == courseCode)
            ?? throw new KeyNotFoundException("Course not found.");
        if (await db.Enrollments.AnyAsync(e => e.LearnerId == learnerId && e.CourseCode == courseCode && e.Status != EnrollmentStatus.Cancelled))
            throw new ArgumentException("Learner is already enrolled in this course.");
        if (course.EnrolledCount >= course.TotalSeats) throw new InvalidOperationException("Course has no available seats.");

        var enrollment = new Enrollment { LearnerId = learnerId, CourseCode = course.CourseCode, EnrollmentDate = DateTime.Today, BaseFee = course.Fee, DiscountFee = 0, FinalFee = course.Fee, Status = EnrollmentStatus.Active, PaymentStatus = PaymentStatus.Pending };
        course.EnrolledCount++;
        db.Enrollments.Add(enrollment);
        await db.SaveChangesAsync();
        return enrollment;
    }

    public async Task CancelAsync(int id)
    {
        var enrollment = await db.Enrollments.FirstOrDefaultAsync(e => e.EnrollmentId == id)
            ?? throw new KeyNotFoundException("Enrollment not found.");
        if (enrollment.Status == EnrollmentStatus.Cancelled) throw new InvalidOperationException("Enrollment is already cancelled.");
        var course = await db.Courses.FirstOrDefaultAsync(c => c.CourseCode == enrollment.CourseCode);
        if (course != null && course.EnrolledCount > 0) course.EnrolledCount--;
        enrollment.Status = EnrollmentStatus.Cancelled;
        await db.SaveChangesAsync();
    }
}
