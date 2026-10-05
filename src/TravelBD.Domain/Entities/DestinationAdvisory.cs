namespace TravelBD.Domain.Entities;

public class DestinationAdvisory
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid LocationId { get; set; }
    public Location Location { get; set; } = null!;

    public string Category { get; set; } = string.Empty;  // "Permits", "Hill Tracts Advisory", "Safety"
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public bool IsMandatory { get; set; }
}
