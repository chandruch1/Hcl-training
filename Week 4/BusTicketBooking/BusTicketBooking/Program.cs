
using BusTicketBooking.Repositories;

PassengerRepository repository = new();

List<BusTicketBooking.Models.Passenger> passengers =
    repository.GetActivePassengers();

if (passengers.Count == 0)
{
    Console.WriteLine("No active passengers found.");
}
else
{
    foreach (var passenger in passengers)
    {
        Console.WriteLine(
            $"{passenger.PassengerId} | " +
            $"{passenger.FullName} | " +
            $"{passenger.Email}");
    }
}
