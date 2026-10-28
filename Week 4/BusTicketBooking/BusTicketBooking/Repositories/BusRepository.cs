
using BusTicketBooking.Models;
using Microsoft.Data.SqlClient;

namespace BusTicketBooking.Repositories;

public class BusRepository
{
    private readonly string _connectionString =
        @"Server=.\SQLEXPRESS;Database=BusTicketDB;Trusted_Connection=True;TrustServerCertificate=True;";

    public int Add(Bus bus)
    {
        const string query = """
            INSERT INTO Buses
                (BusNumber, BusName, TotalSeats, BusType)
            VALUES
                (@BusNumber, @BusName, @TotalSeats, @BusType);

            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """;

        using SqlConnection connection =
            new SqlConnection(_connectionString);

        using SqlCommand command = new SqlCommand(query, connection);

        command.Parameters.AddWithValue("@BusNumber", bus.BusNumber);
        command.Parameters.AddWithValue("@BusName", bus.BusName);
        command.Parameters.AddWithValue("@TotalSeats", bus.TotalSeats);
        command.Parameters.AddWithValue("@BusType", bus.BusType);

        connection.Open();

        return (int)command.ExecuteScalar()!;
    }


    public List<Bus> GetAll()
    {
        const string query = """
        SELECT BusId, BusNumber, BusName,
               TotalSeats, BusType, IsActive
        FROM Buses
        ORDER BY BusId;
        """;

        List<Bus> buses = new();

        using SqlConnection connection =
            new SqlConnection(_connectionString);

        using SqlCommand command = new SqlCommand(query, connection);

        connection.Open();

        using SqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            buses.Add(new Bus
            {
                BusId = reader.GetInt32(0),
                BusNumber = reader.GetString(1),
                BusName = reader.GetString(2),
                TotalSeats = reader.GetInt32(3),
                BusType = reader.GetString(4),
                IsActive = reader.GetBoolean(5)
            });
        }

        return buses;
    }

}
