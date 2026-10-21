using StudentApi.Models;

namespace StudentApi.Services;

public class StudentService
{
    private readonly List<Student> _students =
    [
        new Student
        {
            Id = 1,
            Name = "Arun",
            Department = "CSBS"
        },
        new Student
        {
            Id = 2,
            Name = "Priya",
            Department = "CSE"
        }
    ];

    public List<Student> GetAll()
    {
        return _students;
    }

    public Student? GetById(int id)
    {
        return _students.FirstOrDefault(s => s.Id == id);
    }
}