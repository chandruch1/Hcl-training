namespace AdoNetStudentApp.Models;

public class Student
{
    public int StudentId { get; set; }

    public string Name { get; set; } = string.Empty;

    public int Age { get; set; }

    public string Department { get; set; } = string.Empty;
}