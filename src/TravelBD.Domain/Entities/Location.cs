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

    // Navigation properties
    public ICollection<RouteSegment> OutgoingRoutes { get; set; } = new List<RouteSegment>();
    public ICollection<RouteSegment> IncomingRoutes { get; set; } = new List<RouteSegment>();
    public ICollection<Attraction> Attractions { get; set; } = new List<Attraction>();
    public ICollection<Accommodation> Accommodations { get; set; } = new List<Accommodation>();
    public ICollection<DestinationAdvisory> Advisories { get; set; } = new List<DestinationAdvisory>();
    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
