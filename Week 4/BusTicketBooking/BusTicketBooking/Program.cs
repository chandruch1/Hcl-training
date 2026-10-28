
using BusTicketBooking.Repositories;

PassengerRepository repository = new PassengerRepository();

Console.Write("Enter passenger email: ");
string email = Console.ReadLine() ?? string.Empty;

var passenger = repository.GetByEmail(email);

if (passenger is not null)
{
    Console.WriteLine($"ID: {passenger.PassengerId}");
    Console.WriteLine($"Name: {passenger.FullName}");
    Console.WriteLine($"Email: {passenger.Email}");
    Console.WriteLine($"Active: {passenger.IsActive}");
}
else
{
    Console.WriteLine("Passenger not found.");
}
