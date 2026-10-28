
namespace BusTicketBooking.Models;

public class Booking
{
    public int BookingId { get; set; }

    public int PassengerId { get; set; }

    public int TripId { get; set; }

    public DateTime BookingDate { get; set; }

    public int NumberOfSeats { get; set; }

    public decimal TotalAmount { get; set; }

    public string Status { get; set; } = "Confirmed";
}
