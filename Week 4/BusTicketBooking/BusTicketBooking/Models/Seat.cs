
namespace BusTicketBooking.Models;

public class Seat
{
    public int SeatId { get; set; }

    public int BusId { get; set; }

    public string SeatNumber { get; set; } = string.Empty;

    public string SeatType { get; set; } = "Regular";

    public bool IsActive { get; set; } = true;
}
