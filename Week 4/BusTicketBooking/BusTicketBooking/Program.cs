
using BusTicketBooking.Models;
using BusTicketBooking.Repositories;

BusRepository repository = new();

Bus bus = new()
{
    BusNumber = "TN58AB1234",
    BusName = "Madurai Express",
    TotalSeats = 40,
    BusType = "AC"
};

try
{
    int busId = repository.Add(bus);

    Console.WriteLine(
        $"Bus added successfully. Bus ID: {busId}");
}
catch (Microsoft.Data.SqlClient.SqlException ex)
{
    Console.WriteLine($"Database error: {ex.Message}");
}
