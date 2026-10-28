
using BusTicketBooking.Repositories;

BusRepository repository = new();

List<BusTicketBooking.Models.Bus> buses = repository.GetAll();

if (buses.Count == 0)
{
    Console.WriteLine("No buses found.");
}
else
{
    foreach (var bus in buses)
    {
        Console.WriteLine(
            $"{bus.BusId} | {bus.BusNumber} | " +
            $"{bus.BusName} | {bus.TotalSeats} seats | " +
            $"{bus.BusType} | Active: {bus.IsActive}");
    }
}
