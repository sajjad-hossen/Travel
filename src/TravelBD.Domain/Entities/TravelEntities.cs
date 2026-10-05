using TravelBD.Domain.Enums;

namespace TravelBD.Domain.Entities;

public class Location
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string? BanglaName { get; set; }
    public string Slug { get; set; } = string.Empty;
    public LocationType Type { get; set; } = LocationType.TouristSpot;
    public string District { get; set; } = string.Empty;
    public string Division { get; set; } = string.Empty;
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public bool IsMajorHub { get; set; }
    public bool IsTouristDestination { get; set; }
    public string? Description { get; set; }
    public string? HeroImageUrl { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<RouteSegment> OutgoingRoutes { get; set; } = new List<RouteSegment>();
    public ICollection<RouteSegment> IncomingRoutes { get; set; } = new List<RouteSegment>();
    public ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();
    public ICollection<Accommodation> Accommodations { get; set; } = new List<Accommodation>();
    public ICollection<DestinationAdvisory> Advisories { get; set; } = new List<DestinationAdvisory>();
}

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

    public ICollection<TransportOption> TransportOptions { get; set; } = new List<TransportOption>();
}

public class TransportOption
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid RouteSegmentId { get; set; }
    public RouteSegment RouteSegment { get; set; } = null!;

    public TransportMode Mode { get; set; }
    public RouteTier Tier { get; set; }
    public string OperatorName { get; set; } = string.Empty; // e.g. "Green Line", "Subarna Express", "Local Chander Gari"
    public decimal MinCostBdt { get; set; }
    public decimal MaxCostBdt { get; set; }
    public int FrequencyPerDay { get; set; }
    public string? DepartureStation { get; set; }
    public string? ArrivalStation { get; set; }
    public string? BookingLinksJson { get; set; } // {"Shohoz":"url", "BangladeshRailway":"url"}
    public string? ScheduleNotes { get; set; }    // e.g. "Departs every 45 min", "Night Coach at 11:00 PM"
}

public class Attraction
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid LocationId { get; set; }
    public Location Location { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public string? BanglaName { get; set; }
    public string? Description { get; set; }
    public string? BestTimeToVisit { get; set; }
    public decimal EntryFeeBdt { get; set; }
    public string? ImageUrl { get; set; }
    public string? Category { get; set; } // Nature, Beach, Viewpoint, Waterfalls
}

public class Accommodation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid LocationId { get; set; }
    public Location Location { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public BudgetLevel BudgetLevel { get; set; }
    public string ApproxPriceRange { get; set; } = string.Empty; // e.g. "৳1,500 - ৳3,500/night"
    public string? Address { get; set; }
    public string? ContactPhone { get; set; }
    public string? BookingUrl { get; set; }
    public double Rating { get; set; } = 4.0;
    public string? HighlightFeature { get; set; } // e.g. "Sea view balcony", "Cloud touching hilltop"
}

public class DestinationAdvisory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid LocationId { get; set; }
    public Location Location { get; set; } = null!;

    public string Category { get; set; } = string.Empty; // "Permits", "Hill Tracts Advisory", "Food Speciality", "Safety"
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsMandatory { get; set; }
}
