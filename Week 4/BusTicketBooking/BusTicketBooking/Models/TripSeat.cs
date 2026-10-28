
namespace BusTicketBooking.Models;

public class TripSeat
{
    public int TripSeatId { get; set; }

    public int TripId { get; set; }

    public int SeatId { get; set; }

    public string Status { get; set; } = "Available";
}
