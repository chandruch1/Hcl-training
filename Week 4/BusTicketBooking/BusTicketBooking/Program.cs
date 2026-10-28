
using BusTicketBooking.Models;
using BusTicketBooking.Repositories;

PassengerRepository repository = new PassengerRepository();

Console.Write("Enter passenger ID to update: ");

if (int.TryParse(Console.ReadLine(), out int passengerId))
{
    Passenger? passenger = repository.GetById(passengerId);

    if (passenger is null)
    {
        Console.WriteLine("Passenger not found.");
    }
    else
    {
        Console.Write("Enter new name: ");
        passenger.FullName = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter new email: ");
        passenger.Email = Console.ReadLine() ?? string.Empty;

        Console.Write("Enter new phone: ");
        passenger.Phone = Console.ReadLine();

        bool updated = repository.Update(passenger);

        Console.WriteLine(
            updated
                ? "Passenger updated successfully."
                : "No passenger was updated.");
    }
}
else
{
    Console.WriteLine("Invalid passenger ID.");
}
