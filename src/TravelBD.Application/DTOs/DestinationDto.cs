namespace TravelBD.Application.DTOs;

public record AttractionDto(
    Guid Id,
    string Name,
    string? BanglaName,
    string? Description,
    string? BestTimeToVisit,
    decimal EntryFeeBdt,
    string? ImageUrl,
    string? Category,
    double? Latitude,
    double? Longitude,
    double? DistanceFromTownKm,
    int? TravelTimeMinutes,
    string? HowToReach,
    List<string> GalleryImages,
    double AverageRating,
    int ReviewCount
);

public record AttractionDetailDto(
    Guid Id,
    string Name,
    string? BanglaName,
    string? Description,
    string? BestTimeToVisit,
    decimal EntryFeeBdt,
    string? ImageUrl,
    string? Category,
    double? Latitude,
    double? Longitude,
    double? DistanceFromTownKm,
    int? TravelTimeMinutes,
    string? HowToReach,
    List<string> GalleryImages,
    double AverageRating,
    int ReviewCount,
    Guid LocationId,
    string LocationName,
    double? LocationLatitude,
    double? LocationLongitude,
    List<FeedbackDto> Reviews
);

public record AccommodationDto(
    Guid Id,
    string Name,
    string BudgetLevel,
    string ApproxPriceRange,
    string? Address,
    string? ContactPhone,
    string? BookingUrl,
    double Rating,
    string? HighlightFeature
);

public record AdvisoryDto(
    Guid Id,
    string Category,
    string Title,
    string Content,
    bool IsMandatory
);

public record DestinationDetailDto(
    LocationDto Location,
    List<AttractionDto> Attractions,
    List<AccommodationDto> Accommodations,
    List<AdvisoryDto> Advisories
);
