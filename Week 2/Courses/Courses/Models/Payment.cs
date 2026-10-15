using System;

namespace Courses.Models
{
    public class Payment
    {
        public int PaymentId { get; set; }
        public int EnrollmentId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }
        public PaymentStatus Status { get; set; } 

        public PaymentMode Mode { get; set; }

        public Payment(
            int paymentId,
            int enrollmentId,
            decimal amount,PaymentStatus status,
            PaymentMode Mode)
        {
            this.PaymentId = paymentId;
            this.EnrollmentId = enrollmentId;
            this.Amount = amount;
            this.PaymentDate = DateTime.Now;
            this.Status = status;
            this.Mode = Mode;
        }
    }
}