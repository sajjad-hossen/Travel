using Microsoft.EntityFrameworkCore;
using TravelBD.Application.Interfaces;
using TravelBD.Domain.Entities;
using TravelBD.Infrastructure.Persistence;

namespace TravelBD.Infrastructure.Repositories;

public class AdminRouteRepository : IAdminRouteRepository
{
    private readonly TravelDbContext _context;

    public AdminRouteRepository(TravelDbContext context)
    {
        _context = context;
    }

    public async Task<List<RouteSegment>> GetAllSegmentsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.RouteSegments
            .Include(s => s.Origin)
            .Include(s => s.Destination)
            .Include(s => s.TransportOptions)
            .AsNoTracking()
            .OrderBy(s => s.Origin.Name)
            .ThenBy(s => s.Destination.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<RouteSegment?> GetSegmentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.RouteSegments
            .Include(s => s.Origin)
            .Include(s => s.Destination)
            .Include(s => s.TransportOptions)
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<bool> SegmentExistsAsync(Guid originId, Guid destinationId, Guid? excludeId = null, CancellationToken cancellationToken = default)
    {
        return await _context.RouteSegments
            .AnyAsync(s => s.OriginId == originId && s.DestinationId == destinationId && (!excludeId.HasValue || s.Id != excludeId.Value), cancellationToken);
    }

    public async Task<RouteSegment> CreateSegmentAsync(RouteSegment segment, CancellationToken cancellationToken = default)
    {
        await _context.RouteSegments.AddAsync(segment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return (await GetSegmentByIdAsync(segment.Id, cancellationToken))!;
    }

    public async Task UpdateSegmentAsync(RouteSegment segment, CancellationToken cancellationToken = default)
    {
        _context.RouteSegments.Update(segment);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteSegmentAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var segment = await _context.RouteSegments.FindAsync(new object[] { id }, cancellationToken);
        if (segment == null) return false;

        _context.RouteSegments.Remove(segment);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    // ── Transport Options ─────────────────────────────────────────────────────
    public async Task<TransportOption> AddTransportOptionAsync(TransportOption option, CancellationToken cancellationToken = default)
    {
        await _context.TransportOptions.AddAsync(option, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return option;
    }

    public async Task<TransportOption?> GetTransportOptionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.TransportOptions.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task UpdateTransportOptionAsync(TransportOption option, CancellationToken cancellationToken = default)
    {
        _context.TransportOptions.Update(option);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteTransportOptionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var opt = await _context.TransportOptions.FindAsync(new object[] { id }, cancellationToken);
        if (opt == null) return false;

        _context.TransportOptions.Remove(opt);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
