
namespace BusTicketBooking.Models;

public class Bus
{
    public int BusId { get; set; }

    public string BusNumber { get; set; } = string.Empty;

    public string BusName { get; set; } = string.Empty;

    public int TotalSeats { get; set; }

    public string BusType { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
}
