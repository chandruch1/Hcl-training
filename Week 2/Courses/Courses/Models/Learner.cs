
using System;

namespace Courses.Models
{
    public class Learner
    {
        public int LearnerId { get; set; }
        public string LearnerName { get; set; }
        public string Email { get; set; }
        public long Contact { get; set; }
        public string CourseCode { get; set; }

        public Learner(
            int learnerId,
            string learnerName,
            string email,
            long contact,
            string courseCode)
        {
            if (learnerId <= 0)
                throw new ArgumentException("Enter a valid ID.");

            if (string.IsNullOrWhiteSpace(learnerName))
                throw new ArgumentException("Enter a valid name.");

            if (string.IsNullOrWhiteSpace(email) ||
                !email.Contains("@") ||
                !email.Contains("."))
                throw new ArgumentException("Enter a valid email.");

            if (contact.ToString().Length != 10)
                throw new ArgumentException(
                    "Contact must contain 10 digits.");

            if (string.IsNullOrWhiteSpace(courseCode))
                throw new ArgumentException("Course code is required.");

            LearnerId = learnerId;
            LearnerName = learnerName;
            Email = email;
            Contact = contact;
            CourseCode = courseCode;
        }
    }
}
