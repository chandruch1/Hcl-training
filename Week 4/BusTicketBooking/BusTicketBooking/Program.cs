using BusTicketConsole.Helpers;
using Microsoft.Data.SqlClient;

try
{
    using SqlConnection connection =
        DatabaseHelper.CreateConnection();

    connection.Open();

    Console.WriteLine("Database connection successful!");
    Console.WriteLine($"Server: {connection.DataSource}");
    Console.WriteLine($"Database: {connection.Database}");
}
catch (SqlException ex)
{
    Console.WriteLine("Database connection failed.");
    Console.WriteLine(ex.Message);
}