
using BusTicketBooking.Repositories;

PassengerRepository repository = new PassengerRepository();

Console.Write("Enter passenger ID to deactivate: ");

if (int.TryParse(Console.ReadLine(), out int passengerId))
{
    bool deactivated = repository.Deactivate(passengerId);

    Console.WriteLine(
        deactivated
            ? "Passenger deactivated successfully."
            : "Passenger not found or already inactive.");
}
else
{
    Console.WriteLine("Invalid passenger ID.");
}
