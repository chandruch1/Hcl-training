using System.Data;
using Microsoft.Data.SqlClient;
using AdoNetStudentApp.Models;

namespace AdoNetStudentApp.Repositories;

public class StudentRepository : IStudentRepository
{
    private readonly string _connectionstring;
    public StudentRepository(string connectionstring)
    {
        _connectionstring = connectionstring;
    }
    public int Add( Student student)
    {
        string query = """
            insert into students (Name,Age,Department) values
             (@Name, @Age ,@Department);
            """;

        using SqlConnection connection = new SqlConnection(_connectionstring);
        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.Add("@Name", SqlDbType.VarChar, 100).Value = student.Name;
        command.Parameters.Add("@Age", SqlDbType.Int).Value = student.Age;
        command.Parameters.Add("@Department", SqlDbType.VarChar, 50).Value = student.Department;
        connection.Open();
        return (int)command.ExecuteScalar()!;

    }
    public List<Student> GetAll()
    {
        const string query = """
            SELECT StudentId, Name, Age, Department
            FROM Students
            ORDER BY StudentId;
            """;
        List<Student> students = new List<Student>();
        using SqlConnection connection = new SqlConnection(_connectionstring);
        using SqlCommand command = new SqlCommand(query, connection);
        connection.Open();
        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            students.Add(MapStudent(reader));
        }
        return students;
    }
    private static Student MapStudent(SqlDataReader reader)
    {
        return new Student
        {
            StudentId = reader.GetInt32(0),
            Name = reader.GetString(1),
            Age = reader.GetInt32(2),
            Department = reader.GetString(3)
        };
    }

    public Student? GetById(int id)
    {
        const string query = """
            SELECT StudentId, Name, Age, Department
            FROM Students
            WHERE StudentId = @StudentId;
            """;

        using SqlConnection connection =
            new SqlConnection(_connectionstring);

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.Add("@StudentId", SqlDbType.Int).Value = id;

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            return MapStudent(reader);
        }

        return null;
    }
    

    public bool Update(Student student)
    {
        const string query = """
            UPDATE Students
            SET Name = @Name,
                Age = @Age,
                Department = @Department
            WHERE StudentId = @StudentId;
            """;

        using SqlConnection connection =
            new SqlConnection(_connectionstring);

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.Add("@StudentId", SqlDbType.Int)
            .Value = student.StudentId;

        command.Parameters.Add("@Name", SqlDbType.VarChar, 100)
            .Value = student.Name;

        command.Parameters.Add("@Age", SqlDbType.Int)
            .Value = student.Age;

        command.Parameters.Add("@Department", SqlDbType.VarChar, 50)
            .Value = student.Department;

        connection.Open();

        return command.ExecuteNonQuery() > 0;
    }

    public bool Delete(int id)
    {
        const string query =
            "DELETE FROM Students WHERE StudentId = @StudentId;";

        using SqlConnection connection =
            new SqlConnection(_connectionstring);

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.Add("@StudentId", SqlDbType.Int).Value = id;

        connection.Open();

        return command.ExecuteNonQuery() > 0;
    }
}
