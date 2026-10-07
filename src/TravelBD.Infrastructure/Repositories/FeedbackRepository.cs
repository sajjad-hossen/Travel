using Microsoft.EntityFrameworkCore;
using TravelBD.Application.Interfaces;
using TravelBD.Domain.Entities;
using TravelBD.Infrastructure.Persistence;

namespace TravelBD.Infrastructure.Repositories;

public class FeedbackRepository : IFeedbackRepository
{
    private readonly TravelDbContext _context;

    public FeedbackRepository(TravelDbContext context)
    {
        _context = context;
    }

    public async Task<List<Feedback>> GetAllAsync(Guid? locationId, Guid? attractionId, CancellationToken ct)
    {
        var query = _context.Feedbacks
            .Include(f => f.User)
            .Include(f => f.Location)
            .Include(f => f.Attraction)
            .AsQueryable();

        if (locationId.HasValue)
            query = query.Where(f => f.LocationId == locationId);

        if (attractionId.HasValue)
            query = query.Where(f => f.AttractionId == attractionId);

        return await query.OrderByDescending(f => f.CreatedAt).ToListAsync(ct);
    }

    public async Task AddAsync(Feedback feedback, CancellationToken ct) =>
        await _context.Feedbacks.AddAsync(feedback, ct);

    public Task SaveChangesAsync(CancellationToken ct) =>
        _context.SaveChangesAsync(ct);
}
