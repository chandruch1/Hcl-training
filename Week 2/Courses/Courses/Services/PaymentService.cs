using System;
using System.Collections.Generic;
using Courses.Models;

namespace Courses.Services
{
    public class PaymentService
    {
        private readonly List<Payment> payments = new();
        private readonly CourseService courseService;
        private readonly IPaymentProcessor onlineProcessor;
        private readonly IPaymentProcessor classroomProcessor;

        private int nextPaymentId = 1;

        public PaymentService(
            CourseService courseService,
            IPaymentProcessor onlineProcessor,
            IPaymentProcessor classroomProcessor)
        {
            this.courseService = courseService
                ?? throw new ArgumentNullException(nameof(courseService));

            this.onlineProcessor = onlineProcessor
                ?? throw new ArgumentNullException(nameof(onlineProcessor));

            this.classroomProcessor = classroomProcessor
                ?? throw new ArgumentNullException(nameof(classroomProcessor));
        }

        public void MakePayment(Enrollment enrollment, PaymentMode mode)
        {
            if (enrollment == null)
                throw new ArgumentNullException(nameof(enrollment));

            if (enrollment.Status == EnrollmentStatus.Cancelled)
            {
                Console.WriteLine("Cannot pay for a cancelled enrollment.");
                return;
            }

            IPaymentProcessor processor = mode switch
            {
                PaymentMode.OnlinePayment => onlineProcessor,
                PaymentMode.ClassRoomPayment => classroomProcessor,
                _ => throw new ArgumentOutOfRangeException(nameof(mode))
            };

            Payment payment = new Payment(
                nextPaymentId++,
                enrollment.EnrollmentId,
                enrollment.FinalFee,
                PaymentStatus.Pending,
                mode
            );

            // Save every attempt, including failures.
            payments.Add(payment);

            try
            {
                bool success = processor.ProcessPayment(payment);

                if (success)
                {
                    payment.Status = PaymentStatus.Successful;
                    enrollment.PaymentStatus = PaymentStatus.Successful;
                    Console.WriteLine("Payment successful.");
                }
                else
                {
                    payment.Status = PaymentStatus.Failed;

                    // Only mark the enrollment failed if it isn't
                    // already paid successfully.
                    if (enrollment.PaymentStatus != PaymentStatus.Successful)
                        enrollment.PaymentStatus = PaymentStatus.Failed;

                    Console.WriteLine("Payment failed.");
                }
            }
            catch (Exception ex)
            {
                payment.Status = PaymentStatus.Failed;

                if (enrollment.PaymentStatus != PaymentStatus.Successful)
                    enrollment.PaymentStatus = PaymentStatus.Failed;

                Console.WriteLine($"Payment error: {ex.Message}");
            }
        }

        public void ViewPaymentHistory()
        {
            Console.WriteLine("\n---------- Payment History ----------");

            foreach (Payment payment in payments)
            {
                Console.WriteLine(
                    $"Payment ID: {payment.PaymentId}, " +
                    $"Enrollment ID: {payment.EnrollmentId}, " +
                    $"Amount: {payment.Amount:F2}, " +
                    $"Mode: {payment.Mode}, " +
                    $"Status: {payment.Status}, " +
                    $"Date: {payment.PaymentDate:dd-MM-yyyy HH:mm:ss}");
            }

            Console.WriteLine($"Total attempts: {payments.Count}");
        }
    }
}