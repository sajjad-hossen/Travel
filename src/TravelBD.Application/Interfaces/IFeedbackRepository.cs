using TravelBD.Domain.Entities;

namespace TravelBD.Application.Interfaces;

public interface IFeedbackRepository
{
    Task<List<Feedback>> GetAllAsync(Guid? locationId, Guid? attractionId, CancellationToken ct);
    Task AddAsync(Feedback feedback, CancellationToken ct);
    Task SaveChangesAsync(CancellationToken ct);
}
