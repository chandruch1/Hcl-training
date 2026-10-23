namespace CoursesWebApp.Models;

public enum PaymentMode { OnlinePayment, ClassRoomPayment }

public class Payment
{
    public int PaymentId { get; set; }
    public int EnrollmentId { get; set; }
    public decimal Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public PaymentMode Mode { get; set; }
    public DateTime PaymentDate { get; set; } = DateTime.Now;
}
