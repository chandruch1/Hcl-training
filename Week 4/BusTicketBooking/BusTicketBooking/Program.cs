
using BusTicketBooking.Repositories;

PassengerRepository repository = new PassengerRepository();

Console.Write("Enter passenger ID: ");

if (int.TryParse(Console.ReadLine(), out int passengerId))
{
    var passenger = repository.GetById(passengerId);

    if (passenger is not null)
    {
        Console.WriteLine($"ID: {passenger.PassengerId}");
        Console.WriteLine($"Name: {passenger.FullName}");
        Console.WriteLine($"Email: {passenger.Email}");
        Console.WriteLine($"Phone: {passenger.Phone ?? "No phone"}");
    }
    else
    {
        Console.WriteLine("Passenger not found.");
    }
}
else
{
    Console.WriteLine("Please enter a valid numeric ID.");
}
