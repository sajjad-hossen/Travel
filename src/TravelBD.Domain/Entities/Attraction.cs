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
    public string? ImageUrl { get; set; }
    public string? Category { get; set; }  // Nature, Beach, Viewpoint, Waterfalls
}
