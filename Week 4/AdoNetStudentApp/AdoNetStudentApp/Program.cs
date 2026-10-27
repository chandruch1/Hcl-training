
using Microsoft.Data.SqlClient;
using AdoNetStudentApp.Models;
using AdoNetStudentApp.Repositories;
using AdoNetStudentApp.Services;

namespace AdoNetStudentApp;

class Program
{
    static void Main()
    {
        string connectionString =
            @"Server=.\SQLEXPRESS;Database=StudentDB;Trusted_Connection=True;TrustServerCertificate=True;";

        IStudentRepository repository =
            new StudentRepository(connectionString);

        StudentService service = new StudentService(repository);

        while (true)
        {
            Console.WriteLine("\n===== Student Management System =====");
            Console.WriteLine("1. Add Student");
            Console.WriteLine("2. View All Students");
            Console.WriteLine("3. Search Student by ID");
            Console.WriteLine("4. Update Student");
            Console.WriteLine("5. Delete Student");
            Console.WriteLine("6. Exit");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();

            try
            {
                switch (choice)
                {
                    case "1":
                        AddStudent(service);
                        break;

                    case "2":
                        ViewStudents(service);
                        break;

                    case "3":
                        SearchStudent(service);
                        break;

                    case "4":
                        UpdateStudent(service);
                        break;

                    //case "5":
                    //    DeleteStudent(service);
                    //    break;

                    case "6":
                        Console.WriteLine("Goodbye!");
                        return;

                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
            catch (SqlException ex)
            {
                Console.WriteLine(
                    $"Database error: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Validation error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }

    static void AddStudent(StudentService service)
    {
        Student student = ReadStudentDetails();

        int id = service.AddStudent(student);

        Console.WriteLine(
            $"Student added successfully! ID: {id}");
    }

    static void ViewStudents(StudentService service)
    {
        List<Student> students = service.GetAllStudents();

        if (students.Count == 0)
        {
            Console.WriteLine("No students found.");
            return;
        }

        Console.WriteLine(
            "\n{0,-10}{1,-25}{2,-10}{3,-20}",
            "ID", "Name", "Age", "Department");

        Console.WriteLine(new string('-', 65));

        foreach (Student student in students)
        {
            Console.WriteLine(
                "{0,-10}{1,-25}{2,-10}{3,-20}",
                student.StudentId,
                student.Name,
                student.Age,
                student.Department);
        }
    }

    static void SearchStudent(StudentService service)
    {
        int id = ReadInt("Enter student ID: ");

        Student? student = service.GetStudentById(id);

        if (student == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        DisplayStudent(student);
    }

    static void UpdateStudent(StudentService service)
    {
        int id = ReadInt("Enter student ID to update: ");

        Student? existing = service.GetStudentById(id);

        if (existing == null)
        {
            Console.WriteLine("Student not found.");
            return;
        }

        Console.WriteLine("Enter the updated details:");

        Student updated = ReadStudentDetails();
        updated.StudentId = id;

        bool success = service.UpdateStudent(updated);

        Console.WriteLine(success
            ? "Student updated successfully!"
            : "Student was not updated.");
    }

    //static void DeleteStudent(StudentService service)
    //{
    //    int id = ReadInt("Enter student ID to delete: ");

    //    bool success = service.DeleteStudent(id);

    //    Console.WriteLine(success
    //        ? "Student deleted successfully!"
    //        : "Student not found.");
    //}

    static Student ReadStudentDetails()
    {
        Console.Write("Enter name: ");
        string name = Console.ReadLine() ?? "";

        int age = ReadInt("Enter age: ");

        Console.Write("Enter department: ");
        string department = Console.ReadLine() ?? "";

        return new Student
        {
            Name = name.Trim(),
            Age = age,
            Department = department.Trim()
        };
    }

    static int ReadInt(string message)
    {
        while (true)
        {
            Console.Write(message);

            if (int.TryParse(Console.ReadLine(), out int value))
            {
                return value;
            }

            Console.WriteLine("Please enter a valid number.");
        }
    }

    static void DisplayStudent(Student student)
    {
        Console.WriteLine($"\nStudent ID: {student.StudentId}");
        Console.WriteLine($"Name: {student.Name}");
        Console.WriteLine($"Age: {student.Age}");
        Console.WriteLine($"Department: {student.Department}");
    }
}
