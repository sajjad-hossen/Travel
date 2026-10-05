using Microsoft.EntityFrameworkCore;
using TravelBD.Application.Interfaces;
using TravelBD.Domain.Entities;
using TravelBD.Infrastructure.Persistence;

namespace TravelBD.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of IRouteRepository. Pure data access — no business logic.
/// </summary>
public class RouteRepository : IRouteRepository
{
    private readonly TravelDbContext _context;

    public RouteRepository(TravelDbContext context)
    {
        _context = context;
    }

    public async Task<List<RouteSegment>> GetAllActiveSegmentsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.RouteSegments
            .Include(s => s.Origin)
            .Include(s => s.Destination)
            .Include(s => s.TransportOptions)
            .Where(s => s.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task<Location?> ResolveLocationAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query)) return null;

        var q = query.Trim().ToLower();

        if (Guid.TryParse(query, out var id))
            return await _context.Locations.FindAsync(new object[] { id }, cancellationToken);

        return await _context.Locations
            .AsNoTracking()
            .FirstOrDefaultAsync(
                l => l.Slug.ToLower() == q ||
                     l.Name.ToLower() == q ||
                     (l.BanglaName != null && l.BanglaName == q) ||
                     l.District.ToLower() == q,
                cancellationToken);
    }
}
