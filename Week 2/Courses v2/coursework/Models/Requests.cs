namespace CoursesWebApp.Models;

public class AddCourseRequest
{
    public string CourseCode { get; set; } = "";
    public string Title { get; set; } = "";
    public decimal Fee { get; set; }
    public int DurationDays { get; set; }
    public int TotalSeats { get; set; }
}

public class RegisterLearnerRequest
{
    public int LearnerId { get; set; }
    public string LearnerName { get; set; } = "";
    public string Email { get; set; } = "";
    public long Contact { get; set; }
}

public class CreateEnrollmentRequest
{
    public int LearnerId { get; set; }
    public string CourseCode { get; set; } = "";
}

public class MakePaymentRequest
{
    public int EnrollmentId { get; set; }
    public PaymentMode Mode { get; set; }
}
