
using System;
using System.Collections.Generic;
using System.Linq;
using Courses.Models;

namespace Courses.Services
{
    public class CourseService
    {
        private readonly List<Course> courses = new();

        public void AddCourse(Course newCourse)
        {
            if (newCourse == null)
                throw new ArgumentNullException(nameof(newCourse));

            bool exists = courses.Any(c =>
                string.Equals(
                    c.CourseCode,
                    newCourse.CourseCode,
                    StringComparison.OrdinalIgnoreCase));

            if (exists)
                throw new ArgumentException(
                    $"Course code {newCourse.CourseCode} already exists.");

            courses.Add(newCourse);
            Console.WriteLine(
                $"Course {newCourse.Title} added successfully.");
        }

        public Course FindByCode(string code)
        {
            return courses.FirstOrDefault(c =>
                string.Equals(
                    c.CourseCode,
                    code,
                    StringComparison.OrdinalIgnoreCase));
        }

        public void SearchByCode(string code)
        {
            Course course = FindByCode(code);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return;
            }

            Console.WriteLine($"Course Code: {course.CourseCode}");
            Console.WriteLine($"Course Name: {course.Title}");
            Console.WriteLine($"Fee: {course.Fee}");
            Console.WriteLine(
                $"Available Seats: {course.TotalSeats - course.EnrolledCount}");
        }

        public void DisplayCourses()
        {
            Console.WriteLine(
                $"{"CourseCode",-12} || {"Title",-25} || {"Fees",10} || {"DurationDays",15} || {"TotalSeats",12} || {"AvailableSeats",15}");

            foreach (Course c in courses)
            {
                Console.WriteLine(
                    $"{c.CourseCode,-12} || {c.Title,-25} || {c.Fee,10} || {c.DurationDays,15} || {c.TotalSeats,12} || {c.TotalSeats - c.EnrolledCount,15}");
            }
        }

        public bool DeleteCourse(string courseCode)
        {
            Course course = FindByCode(courseCode);

            if (course == null)
            {
                Console.WriteLine("Course not found.");
                return false;
            }

            if (course.EnrolledCount > 0)
            {
                Console.WriteLine(
                    "Cannot delete a course that has enrolled learners.");
                return false;
            }

            courses.Remove(course);
            Console.WriteLine("Course deleted successfully.");
            return true;
        }
    }
}
