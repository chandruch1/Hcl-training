
using BusTicketBooking.Repositories;

PassengerRepository repository = new PassengerRepository();

List<BusTicketBooking.Models.Passenger> passengers =
    repository.GetAll();

foreach (var passenger in passengers)
{
    Console.WriteLine(
        $"{passenger.PassengerId} | " +
        $"{passenger.FullName} | " +
        $"{passenger.Email} | " +
        $"{passenger.Phone ?? "No phone"}");
}
