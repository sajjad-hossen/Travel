namespace TravelBD.Application.DTOs;

public record CreateFeedbackRequest(
    Guid? LocationId,
    int Rating,
    string Comment
);

public record FeedbackDto(
    Guid Id,
    Guid UserId,
    string UserName,
    Guid? LocationId,
    string? LocationName,
    int Rating,
    string Comment,
    DateTime CreatedAt
);
