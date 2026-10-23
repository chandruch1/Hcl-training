namespace CoursesWebApp.Models;

public enum EnrollmentStatus { Completed, Active, Cancelled }
public enum PaymentStatus { Pending, Completed, Successful, Failed }

public class Enrollment
{
    public int EnrollmentId { get; set; }
    public int LearnerId { get; set; }
    public string CourseCode { get; set; } = "";
    public DateTime EnrollmentDate { get; set; }
    public decimal BaseFee { get; set; }
    public decimal DiscountFee { get; set; }
    public decimal FinalFee { get; set; }
    public EnrollmentStatus Status { get; set; }
    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;
}
