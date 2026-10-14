
using System;

namespace Courses.Models
{
    public class Course
    {
        public string CourseCode { get; set; }
        public string Title { get; set; }
        public decimal Fee { get; set; }
        public int DurationDays { get; set; }
        public int TotalSeats { get; set; }
        public int EnrolledCount { get; set; }

        public Course(
            string courseCode,
            string title,
            decimal fee,
            int durationDays,
            int totalSeats,
            int enrolledCount)
        {
            if (string.IsNullOrWhiteSpace(courseCode))
                throw new ArgumentException("Course code is empty.");

            if (string.IsNullOrWhiteSpace(title))
                throw new ArgumentException("Title is empty.");

            if (fee < 0)
                throw new ArgumentException("Fee cannot be negative.");

            if (durationDays <= 0)
                throw new ArgumentException("Duration must be positive.");

            if (totalSeats <= 0)
                throw new ArgumentException("Total seats must be positive.");

            if (enrolledCount < 0 || enrolledCount > totalSeats)
                throw new ArgumentException("Invalid enrolled count.");

            CourseCode = courseCode;
            Title = title;
            Fee = fee;
            DurationDays = durationDays;
            TotalSeats = totalSeats;
            EnrolledCount = enrolledCount;
        }
    }
}
