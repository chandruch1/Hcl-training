
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using Courses.Models;
using Courses.Exceptions;

namespace Courses.Services
{
    public class EnrollmentService
    {
        private readonly List<Enrollment> enrollments = new();
        private readonly  Dictionary<int,List<string>> Learnermap = new();

        public void AddEnrollment(Enrollment enrollment)
        {
            if (enrollment == null)
                throw new ArgumentNullException(nameof(enrollment));

            enrollments.Add(enrollment);
        }

        public void AddLearnerCourses(string CourseCode,int LearnerId)
        {
            if (Learnermap.ContainsKey(LearnerId)){
                Learnermap[LearnerId].Add(CourseCode);
            }
            else
            {
                Learnermap.Add(
                    LearnerId,
                    new List<string> { CourseCode }
                );
            }
        }

        public void DisplayLearnerList()
        {
            Console.WriteLine("\n-----Learners Courses List ----------");

            foreach(var item in Learnermap)
            {
                Console.WriteLine(
    $"Learner_Id : {item.Key}     LearnerCourses : {string.Join(", ", item.Value)}");
            }
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

        public bool HasEnrollment(int learnerId, string courseCode)
        {
            return enrollments.Any(e =>
                e.LearnerId == learnerId &&
                e.CourseCode == courseCode);
        }

        public void CancelEnrollment(int learnerId, string courseCode)
        {
            Enrollment? enrollment = enrollments.FirstOrDefault(e =>
                e.LearnerId == learnerId &&
                e.CourseCode == courseCode);

            if (enrollment == null)
            {
                throw new CourseCodeandLearnerIdException(
                    "Course code and learner ID not found.");
            }


            enrollments.Remove(enrollment);


            if (Learnermap.TryGetValue(learnerId, out List<string>? courses))
            {
                courses.Remove(courseCode);

                if (courses.Count == 0)
                {
                    Learnermap.Remove(learnerId);
                }
            }

            Console.WriteLine("Enrollment cancelled successfully.");
        }

        public Enrollment? FindEnrollmentById(int enrollmentId)
        {
            return enrollments.FirstOrDefault(e =>
                e.EnrollmentId == enrollmentId);
        }

    }
}
