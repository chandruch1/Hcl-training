
namespace BusTicketBooking.Models;

public class Route
{
    public int RouteId { get; set; }

    public string Source { get; set; } = string.Empty;

    public string Destination { get; set; } = string.Empty;

    public decimal DistanceKm { get; set; }

    public int EstimatedDurationMinutes { get; set; }

    public bool IsActive { get; set; } = true;
}
