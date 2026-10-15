
using System;
using System.Collections.Generic;
using System.Linq;
using Courses.Models;
using Courses.Exceptions;

namespace Courses.Services
{
    public class LearnerService
    {
        private readonly List<Learner> learners = new();

        private readonly CourseService courseService;
        private readonly EnrollmentService enrollmentService;

        private int nextEnrollmentId = 1;

        // Constructor injection
        public LearnerService(
            CourseService courseService,
            EnrollmentService enrollmentService)
        {
            this.courseService = courseService
                ?? throw new ArgumentNullException(nameof(courseService));

            this.enrollmentService = enrollmentService
                ?? throw new ArgumentNullException(nameof(enrollmentService));
        }



        public void Register(Learner newLearner)
        {
            if (newLearner == null)
                throw new ArgumentNullException(nameof(newLearner));

            // Find whether the learner is already registered
            Learner existingLearner =
                learners.FirstOrDefault(l => l.LearnerId == newLearner.LearnerId);

            // Find the requested course
            Course course = courseService.FindByCode(newLearner.CourseCode);

            if (course == null)
            {
                Console.WriteLine("Course is not available.");
                return;
            }

            // Prevent duplicate enrollment in the same course
            if (existingLearner != null &&
                enrollmentService.HasEnrollment(newLearner.LearnerId, course.CourseCode))
            {
                Console.WriteLine("Learner is already enrolled in this course.");
                return;
            }

            // Check available seats
            if (course.EnrolledCount >= course.TotalSeats)
            {
                Console.WriteLine("Course has no available seats.");
                return;
            }

            decimal discount = 0m;
            decimal finalFee = course.Fee - discount;

            // Create an enrollment record
            Enrollment enrollment = new Enrollment(
                nextEnrollmentId,
                newLearner.LearnerId,
                course.CourseCode,
                DateTime.Today,
                course.Fee,
                discount,
                finalFee,
                EnrollmentStatus.Active
            );

            enrollmentService.AddEnrollment(enrollment);
            enrollmentService.AddLearnerCourses(course.CourseCode, newLearner.LearnerId);

            // Add the learner only if they are new
            if (existingLearner == null)
                learners.Add(newLearner);

            course.EnrolledCount++;
            nextEnrollmentId++;

            Console.WriteLine(
                $"Learner {newLearner.LearnerId} enrolled in {course.CourseCode} successfully.");
        }
        // Display all learners
        public void ViewAllLearner()
        {
            Console.WriteLine(
                "\n---------------- Learner List ----------------");

            Console.WriteLine(
                $"{"ID",-8} || {"Name",-20} || {"Contact",-12} || {"Email",-25} || {"Course",-10}");

            foreach (Learner learner in learners)
            {
                Console.WriteLine(
                    $"{learner.LearnerId,-8} || " +
                    $"{learner.LearnerName,-20} || " +
                    $"{learner.Contact,-12} || " +
                    $"{learner.Email,-25} || " +
                    $"{learner.CourseCode,-10}");
            }

            Console.WriteLine($"Total learners: {learners.Count}");
        }

        // Search learner by ID
        public void SearchById(int id)
        {
            Learner learner = learners.FirstOrDefault(
                l => l.LearnerId == id);

            if (learner == null)
            {
                throw new IdnotFoundException("Id is Not Available");
                return;
            }

            Console.WriteLine("\nLearner found:");
            Console.WriteLine($"ID: {learner.LearnerId}");
            Console.WriteLine($"Name: {learner.LearnerName}");
            Console.WriteLine($"Email: {learner.Email}");
            Console.WriteLine($"Contact: {learner.Contact}");
            Console.WriteLine($"Course Code: {learner.CourseCode}");
        }

        // Display learners in descending ID order
        public void OrderByDesc()
        {
            Console.WriteLine(
                "\n---------- Learners by ID (Descending) ----------");

            Console.WriteLine(
                $"{"ID",-8} || {"Name",-20} || {"Contact",-12} || {"Email",-25}");

            foreach (Learner learner in learners.OrderByDescending(
                l => l.LearnerId))
            {
                Console.WriteLine(
                    $"{learner.LearnerId,-8} || " +
                    $"{learner.LearnerName,-20} || " +
                    $"{learner.Contact,-12} || " +
                    $"{learner.Email,-25}");
            }
        }
    }
}
