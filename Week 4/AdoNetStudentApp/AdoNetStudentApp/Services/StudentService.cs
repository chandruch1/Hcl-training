using AdoNetStudentApp.Models;
using AdoNetStudentApp.Repositories;

namespace AdoNetStudentApp.Services;

public class StudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }

    public int AddStudent(Student student)
    {
        ValidateStudent(student);

        return _repository.Add(student);
    }

    
    private static void ValidateStudent(Student student)
    {
        if (string.IsNullOrWhiteSpace(student.Name))
        {
            throw new ArgumentException("Name cannot be empty.");
        }

        if (student.Name.Length > 100)
        {
            throw new ArgumentException(
                "Name cannot exceed 100 characters.");
        }

        if (student.Age <= 0 || student.Age > 150)
        {
            throw new ArgumentException(
                "Age must be between 1 and 150.");
        }

        if (string.IsNullOrWhiteSpace(student.Department))
        {
            throw new ArgumentException(
                "Department cannot be empty.");
        }

        if (student.Department.Length > 50)
        {
            throw new ArgumentException(
                "Department cannot exceed 50 characters.");
        }
    }

    private static void ValidateId(int id)
    {
        if (id <= 0)
        {
            throw new ArgumentException(
                "Student ID must be greater than zero.");
        }
    }
}