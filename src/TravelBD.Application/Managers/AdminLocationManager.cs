using System.Text.Json;
using System.Text.RegularExpressions;
using TravelBD.Application.DTOs;
using TravelBD.Application.Interfaces;
using TravelBD.Domain.Entities;

namespace TravelBD.Application.Managers;

public class AdminLocationManager : IAdminLocationManager
{
    private readonly IAdminLocationRepository _repository;

    public AdminLocationManager(IAdminLocationRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<LocationDto>> GetAllLocationsAsync(CancellationToken cancellationToken = default)
    {
        var list = await _repository.GetAllLocationsAsync(cancellationToken);
        return list.Select(MapLocation).ToList();
    }

    public async Task<LocationDto?> GetLocationByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var loc = await _repository.GetLocationByIdAsync(id, cancellationToken);
        return loc == null ? null : MapLocation(loc);
    }

    public async Task<LocationDto> CreateLocationAsync(CreateLocationRequest request, CancellationToken cancellationToken = default)
    {
        string slug = string.IsNullOrWhiteSpace(request.Slug)
            ? GenerateSlug(request.Name)
            : GenerateSlug(request.Slug);

        // Ensure unique slug
        var existing = await _repository.GetLocationBySlugAsync(slug, cancellationToken);
        if (existing != null)
        {
            slug = $"{slug}-{Guid.NewGuid().ToString()[..6]}";
        }

        var loc = new Location
        {
            Name = request.Name.Trim(),
            BanglaName = request.BanglaName?.Trim(),
            Slug = slug,
            Type = request.Type,
            District = request.District.Trim(),
            Division = request.Division.Trim(),
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            IsMajorHub = request.IsMajorHub,
            IsTouristDestination = request.IsTouristDestination,
            Description = request.Description?.Trim(),
            HeroImageUrl = request.HeroImageUrl?.Trim(),
            CreatedAt = DateTime.UtcNow
        };

        var created = await _repository.CreateLocationAsync(loc, cancellationToken);
        return MapLocation(created);
    }

    public async Task<LocationDto?> UpdateLocationAsync(Guid id, UpdateLocationRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetLocationByIdAsync(id, cancellationToken);
        if (existing == null) return null;

        string slug = GenerateSlug(request.Slug);
        var conflict = await _repository.GetLocationBySlugAsync(slug, cancellationToken);
        if (conflict != null && conflict.Id != id)
        {
            slug = $"{slug}-{Guid.NewGuid().ToString()[..6]}";
        }

        existing.Name = request.Name.Trim();
        existing.BanglaName = request.BanglaName?.Trim();
        existing.Slug = slug;
        existing.Type = request.Type;
        existing.District = request.District.Trim();
        existing.Division = request.Division.Trim();
        existing.Latitude = request.Latitude;
        existing.Longitude = request.Longitude;
        existing.IsMajorHub = request.IsMajorHub;
        existing.IsTouristDestination = request.IsTouristDestination;
        existing.Description = request.Description?.Trim();
        existing.HeroImageUrl = request.HeroImageUrl?.Trim();

        await _repository.UpdateLocationAsync(existing, cancellationToken);
        return MapLocation(existing);
    }

    public Task<bool> DeleteLocationAsync(Guid id, CancellationToken cancellationToken = default)
        => _repository.DeleteLocationAsync(id, cancellationToken);

    // ── Attractions ───────────────────────────────────────────────────────────
    public async Task<AttractionDto> AddAttractionAsync(CreateAttractionRequest request, CancellationToken cancellationToken = default)
    {
        var attraction = new Attraction
        {
            LocationId = request.LocationId,
            Name = request.Name.Trim(),
            BanglaName = request.BanglaName?.Trim(),
            Description = request.Description?.Trim(),
            BestTimeToVisit = request.BestTimeToVisit?.Trim(),
            EntryFeeBdt = Math.Max(0, request.EntryFeeBdt),
            ImageUrl = request.ImageUrl?.Trim(),
            Category = request.Category?.Trim(),
            Latitude = request.Latitude,
            Longitude = request.Longitude,
            DistanceFromTownKm = request.DistanceFromTownKm,
            TravelTimeMinutes = request.TravelTimeMinutes,
            HowToReach = request.HowToReach?.Trim(),
            GalleryImagesJson = SerializeGallery(request.GalleryImages)
        };

        var created = await _repository.AddAttractionAsync(attraction, cancellationToken);
        return MapAttraction(created);
    }

    public async Task<AttractionDto?> UpdateAttractionAsync(Guid id, UpdateAttractionRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetAttractionByIdAsync(id, cancellationToken);
        if (existing == null) return null;

        existing.Name = request.Name.Trim();
        existing.BanglaName = request.BanglaName?.Trim();
        existing.Description = request.Description?.Trim();
        existing.BestTimeToVisit = request.BestTimeToVisit?.Trim();
        existing.EntryFeeBdt = Math.Max(0, request.EntryFeeBdt);
        existing.ImageUrl = request.ImageUrl?.Trim();
        existing.Category = request.Category?.Trim();
        existing.Latitude = request.Latitude;
        existing.Longitude = request.Longitude;
        existing.DistanceFromTownKm = request.DistanceFromTownKm;
        existing.TravelTimeMinutes = request.TravelTimeMinutes;
        existing.HowToReach = request.HowToReach?.Trim();
        existing.GalleryImagesJson = SerializeGallery(request.GalleryImages);

        await _repository.UpdateAttractionAsync(existing, cancellationToken);
        return MapAttraction(existing);
    }

    public Task<bool> DeleteAttractionAsync(Guid id, CancellationToken cancellationToken = default)
        => _repository.DeleteAttractionAsync(id, cancellationToken);

    // ── Accommodations ────────────────────────────────────────────────────────
    public async Task<AccommodationDto> AddAccommodationAsync(CreateAccommodationRequest request, CancellationToken cancellationToken = default)
    {
        var accommodation = new Accommodation
        {
            LocationId = request.LocationId,
            Name = request.Name.Trim(),
            BudgetLevel = request.BudgetLevel,
            ApproxPriceRange = request.ApproxPriceRange.Trim(),
            Address = request.Address?.Trim(),
            ContactPhone = request.ContactPhone?.Trim(),
            BookingUrl = request.BookingUrl?.Trim(),
            Rating = Math.Clamp(request.Rating, 1.0, 5.0),
            HighlightFeature = request.HighlightFeature?.Trim()
        };

        var created = await _repository.AddAccommodationAsync(accommodation, cancellationToken);
        return MapAccommodation(created);
    }

    public async Task<AccommodationDto?> UpdateAccommodationAsync(Guid id, UpdateAccommodationRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetAccommodationByIdAsync(id, cancellationToken);
        if (existing == null) return null;

        existing.Name = request.Name.Trim();
        existing.BudgetLevel = request.BudgetLevel;
        existing.ApproxPriceRange = request.ApproxPriceRange.Trim();
        existing.Address = request.Address?.Trim();
        existing.ContactPhone = request.ContactPhone?.Trim();
        existing.BookingUrl = request.BookingUrl?.Trim();
        existing.Rating = Math.Clamp(request.Rating, 1.0, 5.0);
        existing.HighlightFeature = request.HighlightFeature?.Trim();

        await _repository.UpdateAccommodationAsync(existing, cancellationToken);
        return MapAccommodation(existing);
    }

    public Task<bool> DeleteAccommodationAsync(Guid id, CancellationToken cancellationToken = default)
        => _repository.DeleteAccommodationAsync(id, cancellationToken);

    // ── Advisories ────────────────────────────────────────────────────────────
    public async Task<AdvisoryDto> AddAdvisoryAsync(CreateAdvisoryRequest request, CancellationToken cancellationToken = default)
    {
        var advisory = new DestinationAdvisory
        {
            LocationId = request.LocationId,
            Category = request.Category.Trim(),
            Title = request.Title.Trim(),
            Content = request.Content.Trim(),
            IsMandatory = request.IsMandatory
        };

        var created = await _repository.AddAdvisoryAsync(advisory, cancellationToken);
        return MapAdvisory(created);
    }

    public async Task<AdvisoryDto?> UpdateAdvisoryAsync(Guid id, UpdateAdvisoryRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetAdvisoryByIdAsync(id, cancellationToken);
        if (existing == null) return null;

        existing.Category = request.Category.Trim();
        existing.Title = request.Title.Trim();
        existing.Content = request.Content.Trim();
        existing.IsMandatory = request.IsMandatory;

        await _repository.UpdateAdvisoryAsync(existing, cancellationToken);
        return MapAdvisory(existing);
    }

    public Task<bool> DeleteAdvisoryAsync(Guid id, CancellationToken cancellationToken = default)
        => _repository.DeleteAdvisoryAsync(id, cancellationToken);

    // ── Helpers ───────────────────────────────────────────────────────────────
    private static string GenerateSlug(string text)
    {
        var str = text.ToLowerInvariant().Trim();
        str = Regex.Replace(str, @"\s+", "-");
        str = Regex.Replace(str, @"[^a-z0-9\-]", "");
        return str.Trim('-');
    }

    private static LocationDto MapLocation(Location l) => new(
        l.Id, l.Name, l.BanglaName, l.Slug, l.Type.ToString(),
        l.District, l.Division, l.Latitude, l.Longitude,
        l.IsMajorHub, l.IsTouristDestination, l.Description, l.HeroImageUrl
    );

    private static string? SerializeGallery(List<string>? gallery) =>
        gallery == null || gallery.Count == 0 ? null : JsonSerializer.Serialize(gallery);

    private static List<string> DeserializeGallery(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return new List<string>();
        try { return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>(); }
        catch { return new List<string>(); }
    }

    private static AttractionDto MapAttraction(Attraction a) => new(
        a.Id, a.Name, a.BanglaName, a.Description, a.BestTimeToVisit,
        a.EntryFeeBdt, a.ImageUrl, a.Category,
        a.Latitude, a.Longitude, a.DistanceFromTownKm, a.TravelTimeMinutes, a.HowToReach,
        DeserializeGallery(a.GalleryImagesJson),
        0, 0
    );

    private static AccommodationDto MapAccommodation(Accommodation ac) => new(
        ac.Id, ac.Name, ac.BudgetLevel.ToString(), ac.ApproxPriceRange,
        ac.Address, ac.ContactPhone, ac.BookingUrl, ac.Rating, ac.HighlightFeature
    );

    private static AdvisoryDto MapAdvisory(DestinationAdvisory ad) => new(
        ad.Id, ad.Category, ad.Title, ad.Content, ad.IsMandatory
    );
}
