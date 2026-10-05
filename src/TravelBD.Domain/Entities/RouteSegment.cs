namespace TravelBD.Domain.Entities;

public class RouteSegment
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid OriginId { get; set; }
    public Location Origin { get; set; } = null!;

    public Guid DestinationId { get; set; }
    public Location Destination { get; set; } = null!;

    public double DistanceKm { get; set; }
    public int AvgDurationMinutes { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public ICollection<TransportOption> TransportOptions { get; set; } = new List<TransportOption>();
}
