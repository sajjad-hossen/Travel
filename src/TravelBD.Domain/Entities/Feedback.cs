namespace TravelBD.Domain.Entities;

public class Feedback
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; }
    public User? User { get; set; }
    public Guid? LocationId { get; set; }
    public Location? Location { get; set; }
    public Guid? AttractionId { get; set; }
    public Attraction? Attraction { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
