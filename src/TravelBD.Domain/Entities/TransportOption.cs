using TravelBD.Domain.Enums;

namespace TravelBD.Domain.Entities;

public class TransportOption
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RouteSegmentId { get; set; }
    public RouteSegment RouteSegment { get; set; } = null!;

    public TransportMode Mode { get; set; }
    public RouteTier Tier { get; set; }
    public string OperatorName { get; set; } = string.Empty;  // e.g. "Green Line", "Subarna Express"
    public decimal MinCostBdt { get; set; }
    public decimal MaxCostBdt { get; set; }
    public int FrequencyPerDay { get; set; }
    public string? DepartureStation { get; set; }
    public string? ArrivalStation { get; set; }
    public string? BookingLinksJson { get; set; }  // {"Shohoz":"url", "BangladeshRailway":"url"}
    public string? ScheduleNotes { get; set; }     // e.g. "Departs every 45 min"
}
