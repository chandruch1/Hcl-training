
namespace BusTicketBooking.Models;

public class BookingSeat
{
    public int BookingSeatId { get; set; }

    public int BookingId { get; set; }

    public int SeatId { get; set; }

    public decimal SeatPrice { get; set; }
}
