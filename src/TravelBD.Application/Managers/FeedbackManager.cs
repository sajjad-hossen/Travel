using TravelBD.Application.DTOs;
using TravelBD.Application.Interfaces;
using TravelBD.Domain.Entities;

namespace TravelBD.Application.Managers;

public class FeedbackManager : IFeedbackManager
{
    private readonly IFeedbackRepository _feedbacks;
    private readonly IUserRepository _users;

    public FeedbackManager(IFeedbackRepository feedbacks, IUserRepository users)
    {
        _feedbacks = feedbacks;
        _users = users;
    }

    public async Task<List<FeedbackDto>> GetAllAsync(Guid? locationId, CancellationToken ct)
    {
        var feedbacks = await _feedbacks.GetAllAsync(locationId, ct);
        return feedbacks.Select(ToDto).ToList();
    }

    public async Task<FeedbackDto> CreateAsync(Guid userId, CreateFeedbackRequest request, CancellationToken ct)
    {
        if (request.Rating is < 1 or > 5)
            throw new ArgumentException("Rating must be between 1 and 5.");

        var user = await _users.GetByIdAsync(userId, ct)
            ?? throw new UnauthorizedAccessException("User not found.");

        var feedback = new Feedback
        {
            UserId = userId,
            LocationId = request.LocationId,
            Rating = request.Rating,
            Comment = request.Comment.Trim()
        };

        await _feedbacks.AddAsync(feedback, ct);
        await _feedbacks.SaveChangesAsync(ct);

        feedback.User = user;
        return ToDto(feedback);
    }

    private static FeedbackDto ToDto(Feedback f) => new(
        f.Id,
        f.UserId,
        f.User?.Name ?? string.Empty,
        f.LocationId,
        f.Location?.Name,
        f.Rating,
        f.Comment,
        f.CreatedAt
    );
}
