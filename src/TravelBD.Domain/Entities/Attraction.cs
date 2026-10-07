namespace TravelBD.Domain.Entities;

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
    public string? ImageUrl { get; set; } // cover photo
    public string? GalleryImagesJson { get; set; } // JSON array of extra image URLs
    public string? Category { get; set; }  // Nature, Beach, Viewpoint, Waterfalls

    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public double? DistanceFromTownKm { get; set; }
    public int? TravelTimeMinutes { get; set; }
    public string? HowToReach { get; set; }

    public ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();
}
