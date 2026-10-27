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
        connection.Open(
        return (int)command.ExecuteScalar()!;

    }
}
