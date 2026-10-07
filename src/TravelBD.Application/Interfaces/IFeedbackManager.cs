using TravelBD.Application.DTOs;

namespace TravelBD.Application.Interfaces;

public interface IFeedbackManager
{
    Task<List<FeedbackDto>> GetAllAsync(Guid? locationId, CancellationToken ct);
    Task<FeedbackDto> CreateAsync(Guid userId, CreateFeedbackRequest request, CancellationToken ct);
}
