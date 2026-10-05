using Microsoft.EntityFrameworkCore;
using TravelBD.Application.Interfaces;
using TravelBD.Domain.Entities;
using TravelBD.Infrastructure.Persistence;

namespace TravelBD.Infrastructure.Repositories;

public class AdminLocationRepository : IAdminLocationRepository
{
    private readonly TravelDbContext _context;

    public AdminLocationRepository(TravelDbContext context)
    {
        _context = context;
    }

    public async Task<List<Location>> GetAllLocationsAsync(CancellationToken cancellationToken = default)
    {
        return await _context.Locations
            .AsNoTracking()
            .OrderBy(l => l.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<Location?> GetLocationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Locations
            .Include(l => l.Attractions)
            .Include(l => l.Accommodations)
            .Include(l => l.Advisories)
            .FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
    }

    public async Task<Location?> GetLocationBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await _context.Locations
            .AsNoTracking()
            .FirstOrDefaultAsync(l => l.Slug.ToLower() == slug.ToLower(), cancellationToken);
    }

    public async Task<Location> CreateLocationAsync(Location location, CancellationToken cancellationToken = default)
    {
        await _context.Locations.AddAsync(location, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return location;
    }

    public async Task UpdateLocationAsync(Location location, CancellationToken cancellationToken = default)
    {
        _context.Locations.Update(location);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteLocationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var loc = await _context.Locations.FindAsync(new object[] { id }, cancellationToken);
        if (loc == null) return false;

        _context.Locations.Remove(loc);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    // ── Attractions ───────────────────────────────────────────────────────────
    public async Task<Attraction> AddAttractionAsync(Attraction attraction, CancellationToken cancellationToken = default)
    {
        await _context.Attractions.AddAsync(attraction, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return attraction;
    }

    public async Task<Attraction?> GetAttractionByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Attractions.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task UpdateAttractionAsync(Attraction attraction, CancellationToken cancellationToken = default)
    {
        _context.Attractions.Update(attraction);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAttractionAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var att = await _context.Attractions.FindAsync(new object[] { id }, cancellationToken);
        if (att == null) return false;

        _context.Attractions.Remove(att);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    // ── Accommodations ────────────────────────────────────────────────────────
    public async Task<Accommodation> AddAccommodationAsync(Accommodation accommodation, CancellationToken cancellationToken = default)
    {
        await _context.Accommodations.AddAsync(accommodation, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return accommodation;
    }

    public async Task<Accommodation?> GetAccommodationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Accommodations.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task UpdateAccommodationAsync(Accommodation accommodation, CancellationToken cancellationToken = default)
    {
        _context.Accommodations.Update(accommodation);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAccommodationAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var acc = await _context.Accommodations.FindAsync(new object[] { id }, cancellationToken);
        if (acc == null) return false;

        _context.Accommodations.Remove(acc);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }

    // ── Advisories ────────────────────────────────────────────────────────────
    public async Task<DestinationAdvisory> AddAdvisoryAsync(DestinationAdvisory advisory, CancellationToken cancellationToken = default)
    {
        await _context.DestinationAdvisories.AddAsync(advisory, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return advisory;
    }

    public async Task<DestinationAdvisory?> GetAdvisoryByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.DestinationAdvisories.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task UpdateAdvisoryAsync(DestinationAdvisory advisory, CancellationToken cancellationToken = default)
    {
        _context.DestinationAdvisories.Update(advisory);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> DeleteAdvisoryAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var adv = await _context.DestinationAdvisories.FindAsync(new object[] { id }, cancellationToken);
        if (adv == null) return false;

        _context.DestinationAdvisories.Remove(adv);
        await _context.SaveChangesAsync(cancellationToken);
        return true;
    }
}
