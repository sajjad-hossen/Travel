using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using TravelBD.Application.DTOs;
using TravelBD.Application.Interfaces;
using TravelBD.Domain.Entities;
using TravelBD.Infrastructure.Persistence;

namespace TravelBD.Infrastructure.Repositories;

/// <summary>
/// EF Core implementation of ILocationRepository. Pure data access — no business logic.
/// </summary>
public class LocationRepository : ILocationRepository
{
    private readonly TravelDbContext _context;

    public LocationRepository(TravelDbContext context)
    {
        _context = context;
    }

    public async Task<List<LocationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var locations = await _context.Locations
            .AsNoTracking()
            .OrderBy(l => l.Name)
            .ToListAsync(cancellationToken);

        return locations.Select(MapToDto).ToList();
    }

    public async Task<List<LocationDto>> SearchAsync(string query, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(query))
            return await GetTouristDestinationsAsync(cancellationToken);

        var q = query.Trim().ToLower();

        var locations = await _context.Locations
            .AsNoTracking()
            .Where(l =>
                l.Name.ToLower().Contains(q) ||
                (l.BanglaName != null && l.BanglaName.Contains(q)) ||
                l.District.ToLower().Contains(q))
            .OrderByDescending(l => l.IsTouristDestination)
            .ThenBy(l => l.Name)
            .Take(10)
            .ToListAsync(cancellationToken);

        return locations.Select(MapToDto).ToList();
    }

    public async Task<List<LocationDto>> GetTouristDestinationsAsync(CancellationToken cancellationToken = default)
    {
        var locations = await _context.Locations
            .AsNoTracking()
            .Where(l => l.IsTouristDestination)
            .OrderBy(l => l.Name)
            .ToListAsync(cancellationToken);

        return locations.Select(MapToDto).ToList();
    }

    public async Task<DestinationDetailDto?> GetDetailsBySlugOrIdAsync(
        string slugOrId, CancellationToken cancellationToken = default)
    {
        var query = _context.Locations
            .Include(l => l.Attractions).ThenInclude(a => a.Feedbacks)
            .Include(l => l.Accommodations)
            .Include(l => l.Advisories)
            .AsNoTracking();

        Location? location = Guid.TryParse(slugOrId, out var id)
            ? await query.FirstOrDefaultAsync(l => l.Id == id, cancellationToken)
            : await query.FirstOrDefaultAsync(l => l.Slug.ToLower() == slugOrId.Trim().ToLower(), cancellationToken);

        if (location == null) return null;

        return new DestinationDetailDto(
            MapToDto(location),
            location.Attractions.Select(MapAttraction).ToList(),
            location.Accommodations.Select(ac => new AccommodationDto(
                ac.Id, ac.Name, ac.BudgetLevel.ToString(), ac.ApproxPriceRange,
                ac.Address, ac.ContactPhone, ac.BookingUrl, ac.Rating, ac.HighlightFeature, ac.ImageUrl
            )).ToList(),
            location.Advisories.Select(ad => new AdvisoryDto(
                ad.Id, ad.Category, ad.Title, ad.Content, ad.IsMandatory
            )).ToList()
        );
    }

    public async Task<Attraction?> GetAttractionDetailAsync(Guid attractionId, CancellationToken cancellationToken = default)
    {
        return await _context.Attractions
            .Include(a => a.Location)
            .Include(a => a.Feedbacks).ThenInclude(f => f.User)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == attractionId, cancellationToken);
    }

    private static AttractionDto MapAttraction(Attraction a) => new(
        a.Id, a.Name, a.BanglaName, a.Description, a.BestTimeToVisit,
        a.EntryFeeBdt, a.ImageUrl, a.Category,
        a.Latitude, a.Longitude, a.DistanceFromTownKm, a.TravelTimeMinutes, a.HowToReach,
        DeserializeGallery(a.GalleryImagesJson),
        a.Feedbacks.Count == 0 ? 0 : Math.Round(a.Feedbacks.Average(f => f.Rating), 1),
        a.Feedbacks.Count
    );

    private static List<string> DeserializeGallery(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return new List<string>();
        try { return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>(); }
        catch { return new List<string>(); }
    }

    private static LocationDto MapToDto(Location l) => new(
        l.Id, l.Name, l.BanglaName, l.Slug, l.Type.ToString(),
        l.District, l.Division, l.Latitude, l.Longitude,
        l.IsMajorHub, l.IsTouristDestination, l.Description, l.HeroImageUrl
    );
}
