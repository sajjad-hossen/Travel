using TravelBD.Domain.Enums;

namespace TravelBD.Domain.Entities;

public class Accommodation
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid LocationId { get; set; }
    public Location Location { get; set; } = null!;

    public string Name { get; set; } = string.Empty;
    public BudgetLevel BudgetLevel { get; set; }
    public string ApproxPriceRange { get; set; } = string.Empty;  // e.g. "৳1,500 - ৳3,500/night"
    public string? Address { get; set; }
    public string? ContactPhone { get; set; }
    public string? BookingUrl { get; set; }
    public double Rating { get; set; } = 4.0;
    public string? HighlightFeature { get; set; }  // e.g. "Sea view balcony"
}
