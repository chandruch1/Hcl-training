
using System;
using Courses.Models;
using Courses.Services;

class Program
{
    static void Main(string[] args)
    {
        // Create service objects once
        CourseService courseService = new CourseService();
        EnrollmentService enrollmentService = new EnrollmentService();

        // Inject the existing service objects
        LearnerService learnerService = new LearnerService(
            courseService,
            enrollmentService);

        try
        {
            // Add courses
            courseService.AddCourse(
                new Course("101", "C#", 5000m, 30, 400, 12));

            courseService.AddCourse(
                new Course("102", "Python Fullstack", 5010m, 20, 20, 10));

            courseService.AddCourse(
                new Course("103", "Java FullStack", 3000m, 100, 150, 120));

            courseService.AddCourse(
                new Course("104", "CloudDeployment", 3500m, 100, 100, 43));

            courseService.AddCourse(
                new Course("105", "MERN Stack", 3400m, 60, 600, 150));

            courseService.AddCourse(
                new Course("106", "Prompt Engineering", 3000m, 40, 400, 24));

            courseService.AddCourse(
                new Course("107", "DevOps", 3200m, 25, 500, 120));

            // Display courses
            Console.WriteLine("\n========== ALL COURSES ==========");
            courseService.DisplayCourses();

            // Register learners
            Console.WriteLine("\n========== REGISTER LEARNERS ==========");

            learnerService.Register(
                new Learner(101, "Arun Kumar", "arun@gmail.com",
                    9876543210L, "101"));

            learnerService.Register(
                new Learner(102, "Priya", "priya@yahoo.com",
                    9876543211L, "102"));

            learnerService.Register(
                new Learner(103, "Karthik", "karthik@outlook.com",
                    9876543212L, "103"));

            learnerService.Register(
                new Learner(104, "Divya", "divya@gmail.com",
                    9876543213L, "103"));

            learnerService.Register(
                new Learner(105, "Rahul", "rahul@college.edu",
                    9876543214L, "101"));

            // Display learners
            Console.WriteLine("\n========== ALL LEARNERS ==========");
            learnerService.ViewAllLearner();

            // Display enrollments from the same service instance
            Console.WriteLine("\n========== ALL ENROLLMENTS ==========");
            enrollmentService.ViewEnrollment();

            // Search for a course
            Console.WriteLine("\n========== SEARCH COURSE ==========");
            courseService.SearchByCode("101");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
    }
}
