namespace TravelBD.Application.DTOs;

public record CreateFeedbackRequest(
    Guid? LocationId,
    Guid? AttractionId,
    int Rating,
    string Comment
);

public record FeedbackDto(
    Guid Id,
    Guid UserId,
    string UserName,
    Guid? LocationId,
    string? LocationName,
    Guid? AttractionId,
    string? AttractionName,
    int Rating,
    string Comment,
    DateTime CreatedAt
);
