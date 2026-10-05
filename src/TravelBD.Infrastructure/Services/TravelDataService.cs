using Microsoft.EntityFrameworkCore;
using TravelBD.Application.Common.Models;
using TravelBD.Application.Interfaces;
using TravelBD.Domain.Entities;
using TravelBD.Infrastructure.Persistence;

namespace TravelBD.Infrastructure.Services;

public class TravelDataService : ITravelDataService
{
    private readonly TravelDbContext _context;

    public TravelDataService(TravelDbContext context)
    {
        _context = context;
    }

    public async Task<List<LocationDto>> GetAllLocationsAsync(CancellationToken cancellationToken = default)
    {
        var locations = await _context.Locations
            .OrderBy(l => l.Name)
            .ToListAsync(cancellationToken);

        return locations.Select(MapLocation).ToList();
    }

    public async Task<List<LocationDto>> SearchLocationsAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return await GetTouristDestinationsAsync(cancellationToken);

        var q = query.Trim().ToLower();

        var locations = await _context.Locations
            .Where(l => l.Name.ToLower().Contains(q) ||
                        (l.BanglaName != null && l.BanglaName.Contains(q)) ||
                        l.District.ToLower().Contains(q))
            .OrderByDescending(l => l.IsTouristDestination)
            .ThenBy(l => l.Name)
            .Take(10)
            .ToListAsync(cancellationToken);

        return locations.Select(MapLocation).ToList();
    }

    public async Task<List<LocationDto>> GetTouristDestinationsAsync(CancellationToken cancellationToken = default)
    {
        var locations = await _context.Locations
            .Where(l => l.IsTouristDestination)
            .OrderBy(l => l.Name)
            .ToListAsync(cancellationToken);

        return locations.Select(MapLocation).ToList();
    }

    public async Task<DestinationDetailDto?> GetDestinationDetailsAsync(string slugOrId, CancellationToken cancellationToken = default)
    {
        var query = _context.Locations
            .Include(l => l.Attractions)
            .Include(l => l.Accommodations)
            .Include(l => l.Advisories)
            .AsNoTracking();

        Location? location = null;
        if (Guid.TryParse(slugOrId, out var id))
        {
            location = await query.FirstOrDefaultAsync(l => l.Id == id, cancellationToken);
        }
        else
        {
            var slug = slugOrId.Trim().ToLower();
            location = await query.FirstOrDefaultAsync(l => l.Slug.ToLower() == slug, cancellationToken);
        }

        if (location == null) return null;

        var locDto = MapLocation(location);

        var attractions = location.Attractions.Select(a => new AttractionDto(
            a.Id,
            a.Name,
            a.BanglaName,
            a.Description,
            a.BestTimeToVisit,
            a.EntryFeeBdt,
            a.ImageUrl,
            a.Category
        )).ToList();

        var accommodations = location.Accommodations.Select(ac => new AccommodationDto(
            ac.Id,
            ac.Name,
            ac.BudgetLevel.ToString(),
            ac.ApproxPriceRange,
            ac.Address,
            ac.ContactPhone,
            ac.BookingUrl,
            ac.Rating,
            ac.HighlightFeature
        )).ToList();

        var advisories = location.Advisories.Select(ad => new AdvisoryDto(
            ad.Id,
            ad.Category,
            ad.Title,
            ad.Content,
            ad.IsMandatory
        )).ToList();

        return new DestinationDetailDto(locDto, attractions, accommodations, advisories);
    }

    private static LocationDto MapLocation(Location l)
    {
        return new LocationDto(
            l.Id,
            l.Name,
            l.BanglaName,
            l.Slug,
            l.Type.ToString(),
            l.District,
            l.Division,
            l.Latitude,
            l.Longitude,
            l.IsMajorHub,
            l.IsTouristDestination,
            l.Description,
            l.HeroImageUrl
        );
    }
}
