
using System;

namespace Courses.Models
{
    public class Enrollment
    {
        public int EnrollmentId { get; set; }
        public int LearnerId { get; set; }
        public string CourseCode { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public decimal BaseFee { get; set; }
        public decimal DiscountFee { get; set; }
        public decimal FinalFee { get; set; }
        public EnrollmentStatus Status { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
    = PaymentStatus.Pending;

        public Enrollment(
            int enrollmentId,
            int learnerId,
            string courseCode,
            DateTime enrollmentDate,
            decimal baseFee,
            decimal discountFee,
            decimal finalFee,
            EnrollmentStatus status)
        {
            EnrollmentId = enrollmentId;
            LearnerId = learnerId;
            CourseCode = courseCode;
            EnrollmentDate = enrollmentDate;
            BaseFee = baseFee;
            DiscountFee = discountFee;
            FinalFee = finalFee;
            Status = status;
        }
    }
}
