
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
}
