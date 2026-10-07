using System.Text.Json;
using TravelBD.Application.DTOs;
using TravelBD.Application.Interfaces;
using TravelBD.Domain.Entities;

namespace TravelBD.Application.Managers;

public class AttractionManager : IAttractionManager
{
    private readonly ILocationRepository _locationRepository;

    public AttractionManager(ILocationRepository locationRepository)
    {
        _locationRepository = locationRepository;
    }

    public async Task<AttractionDetailDto?> GetDetailAsync(Guid attractionId, CancellationToken cancellationToken = default)
    {
        var attraction = await _locationRepository.GetAttractionDetailAsync(attractionId, cancellationToken);
        if (attraction == null) return null;

        var reviews = attraction.Feedbacks
            .OrderByDescending(f => f.CreatedAt)
            .Select(f => new FeedbackDto(
                f.Id, f.UserId, f.User?.Name ?? string.Empty,
                f.LocationId, null, f.AttractionId, attraction.Name,
                f.Rating, f.Comment, f.CreatedAt
            ))
            .ToList();

        var averageRating = attraction.Feedbacks.Count == 0
            ? 0
            : Math.Round(attraction.Feedbacks.Average(f => f.Rating), 1);

        return new AttractionDetailDto(
            attraction.Id,
            attraction.Name,
            attraction.BanglaName,
            attraction.Description,
            attraction.BestTimeToVisit,
            attraction.EntryFeeBdt,
            attraction.ImageUrl,
            attraction.Category,
            attraction.Latitude,
            attraction.Longitude,
            attraction.DistanceFromTownKm,
            attraction.TravelTimeMinutes,
            attraction.HowToReach,
            DeserializeGallery(attraction.GalleryImagesJson),
            averageRating,
            attraction.Feedbacks.Count,
            attraction.LocationId,
            attraction.Location.Name,
            attraction.Location.Latitude,
            attraction.Location.Longitude,
            reviews
        );
    }

    private static List<string> DeserializeGallery(string? json)
    {
        if (string.IsNullOrWhiteSpace(json)) return new List<string>();
        try { return JsonSerializer.Deserialize<List<string>>(json) ?? new List<string>(); }
        catch { return new List<string>(); }
    }
}
