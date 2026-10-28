
using Microsoft.Data.SqlClient;
using BusTicketBooking.Models;

namespace BusTicketBooking.Repositories;

public class PassengerRepository
{
    private readonly string _connectionString =
        @"Server=.\SQLEXPRESS;Database=BusTicketDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public int Add(Passenger passenger)
    {
        const string query = """
            INSERT INTO Passengers
                (FullName, Email, Phone)
            VALUES
                (@FullName, @Email, @Phone);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        using SqlConnection connection =
            new SqlConnection(_connectionString);

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@FullName", passenger.FullName);
        command.Parameters.AddWithValue("@Email", passenger.Email);
        command.Parameters.AddWithValue(
            "@Phone",
            (object?)passenger.Phone ?? DBNull.Value);

        connection.Open();

        return (int)command.ExecuteScalar()!;
    }

    public List<Passenger> GetAll()
    {
        const string query = """
        SELECT PassengerId, FullName, Email, Phone,
               CreatedAt, IsActive
        FROM Passengers
        ORDER BY PassengerId;
        """;

        List<Passenger> passengers = new();

        using SqlConnection connection =
            new SqlConnection(_connectionString);

        using SqlCommand command = new SqlCommand(query, connection);

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            passengers.Add(new Passenger
            {
                PassengerId = reader.GetInt32(0),
                FullName = reader.GetString(1),
                Email = reader.GetString(2),
                Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                CreatedAt = reader.GetDateTime(4),
                IsActive = reader.GetBoolean(5)
            });
        }

        return passengers;
    }

    public Passenger? GetById(int passengerId)
    {
        const string query = """
        SELECT PassengerId, FullName, Email, Phone,
               CreatedAt, IsActive
        FROM Passengers
        WHERE PassengerId = @PassengerId;
        """;

        using SqlConnection connection =
            new SqlConnection(_connectionString);

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@PassengerId", passengerId);

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        return new Passenger
        {
            PassengerId = reader.GetInt32(0),
            FullName = reader.GetString(1),
            Email = reader.GetString(2),
            Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
            CreatedAt = reader.GetDateTime(4),
            IsActive = reader.GetBoolean(5)
        };
    }


    public bool Update(Passenger passenger)
    {
        const string query = """
        UPDATE Passengers
        SET FullName = @FullName,
            Email = @Email,
            Phone = @Phone
        WHERE PassengerId = @PassengerId;
        """;

        using SqlConnection connection =
            new SqlConnection(_connectionString);

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@PassengerId", passenger.PassengerId);
        command.Parameters.AddWithValue("@FullName", passenger.FullName);
        command.Parameters.AddWithValue("@Email", passenger.Email);
        command.Parameters.AddWithValue(
            "@Phone",
            (object?)passenger.Phone ?? DBNull.Value);

        connection.Open();

        int rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
    }


    public bool Deactivate(int passengerId)
    {
        const string query = """
        UPDATE Passengers
        SET IsActive = 0
        WHERE PassengerId = @PassengerId
          AND IsActive = 1;
        """;

        using SqlConnection connection =
            new SqlConnection(_connectionString);

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@PassengerId", passengerId);

        connection.Open();

        int rowsAffected = command.ExecuteNonQuery();

        return rowsAffected > 0;
    }


    public Passenger? GetByEmail(string email)
    {
        const string query = """
        SELECT PassengerId, FullName, Email, Phone,
               CreatedAt, IsActive
        FROM Passengers
        WHERE Email = @Email;
        """;

        using SqlConnection connection =
            new SqlConnection(_connectionString);

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@Email", email);

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        if (!reader.Read())
        {
            return null;
        }

        return new Passenger
        {
            PassengerId = reader.GetInt32(0),
            FullName = reader.GetString(1),
            Email = reader.GetString(2),
            Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
            CreatedAt = reader.GetDateTime(4),
            IsActive = reader.GetBoolean(5)
        };
    }


    public List<Passenger> GetActivePassengers()
    {
        const string query = """
        SELECT PassengerId, FullName, Email, Phone,
               CreatedAt, IsActive
        FROM Passengers
        WHERE IsActive = 1
        ORDER BY FullName;
        """;

        List<Passenger> passengers = new();

        using SqlConnection connection =
            new SqlConnection(_connectionString);

        using SqlCommand command = new SqlCommand(query, connection);

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            passengers.Add(new Passenger
            {
                PassengerId = reader.GetInt32(0),
                FullName = reader.GetString(1),
                Email = reader.GetString(2),
                Phone = reader.IsDBNull(3) ? null : reader.GetString(3),
                CreatedAt = reader.GetDateTime(4),
                IsActive = reader.GetBoolean(5)
            });
        }

        return passengers;
    }


}
