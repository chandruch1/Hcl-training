
using System;
using System.Collections.Generic;
using Courses.Models;

namespace Courses.Services
{
    public class EnrollmentService
    {
        private readonly List<Enrollment> enrollments = new();

        public void AddEnrollment(Enrollment enrollment)
        {
            if (enrollment == null)
                throw new ArgumentNullException(nameof(enrollment));

            enrollments.Add(enrollment);
        }

        public void ViewEnrollment()
        {
            Console.WriteLine("\n--------------- Enrollment List ---------------");

            Console.WriteLine(
                $"{"ID",-6} || {"Learner ID",-12} || {"Course Code",-12} || {"Date",-12} || {"Base Fee",10} || {"Discount",10} || {"Final Fee",10} || {"Status",-10}");

            foreach (Enrollment e in enrollments)
            {
                Console.WriteLine(
                    $"{e.EnrollmentId,-6} || {e.LearnerId,-12} || {e.CourseCode,-12} || {e.EnrollmentDate:dd-MM-yyyy} || {e.BaseFee,10:F2} || {e.DiscountFee,10:F2} || {e.FinalFee,10:F2} || {e.Status,-10}");
            }

            Console.WriteLine($"Total enrollments: {enrollments.Count}");
        }
    }
}
