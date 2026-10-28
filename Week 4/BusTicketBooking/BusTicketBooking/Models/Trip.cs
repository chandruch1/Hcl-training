
namespace BusTicketBooking.Models;

public class Trip
{
    public int TripId { get; set; }

    public int BusId { get; set; }

    public int RouteId { get; set; }

    public DateTime DepartureTime { get; set; }

    public DateTime ArrivalTime { get; set; }

    public decimal TicketPrice { get; set; }

    public int AvailableSeats { get; set; }

    public string Status { get; set; } = "Scheduled";
}
