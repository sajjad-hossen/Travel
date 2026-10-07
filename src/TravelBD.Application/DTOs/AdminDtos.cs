using TravelBD.Domain.Enums;

namespace TravelBD.Application.DTOs;

// ── Locations ─────────────────────────────────────────────────────────────────
public record CreateLocationRequest(
    string Name,
    string? BanglaName,
    string? Slug,
    LocationType Type,
    string District,
    string Division,
    double? Latitude,
    double? Longitude,
    bool IsMajorHub,
    bool IsTouristDestination,
    string? Description,
    string? HeroImageUrl
);

public record UpdateLocationRequest(
    string Name,
    string? BanglaName,
    string Slug,
    LocationType Type,
    string District,
    string Division,
    double? Latitude,
    double? Longitude,
    bool IsMajorHub,
    bool IsTouristDestination,
    string? Description,
    string? HeroImageUrl
);

// ── Attractions ───────────────────────────────────────────────────────────────
public record CreateAttractionRequest(
    Guid LocationId,
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
    List<string>? GalleryImages
);

public record UpdateAttractionRequest(
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
    List<string>? GalleryImages
);

// ── Accommodations ────────────────────────────────────────────────────────────
public record CreateAccommodationRequest(
    Guid LocationId,
    string Name,
    BudgetLevel BudgetLevel,
    string ApproxPriceRange,
    string? Address,
    string? ContactPhone,
    string? BookingUrl,
    double Rating,
    string? HighlightFeature
);

public record UpdateAccommodationRequest(
    string Name,
    BudgetLevel BudgetLevel,
    string ApproxPriceRange,
    string? Address,
    string? ContactPhone,
    string? BookingUrl,
    double Rating,
    string? HighlightFeature
);

// ── Advisories ────────────────────────────────────────────────────────────────
public record CreateAdvisoryRequest(
    Guid LocationId,
    string Category,
    string Title,
    string Content,
    bool IsMandatory
);

public record UpdateAdvisoryRequest(
    string Category,
    string Title,
    string Content,
    bool IsMandatory
);

// ── Routes & Transport ────────────────────────────────────────────────────────
public record CreateRouteSegmentRequest(
    Guid OriginId,
    Guid DestinationId,
    double DistanceKm,
    int AvgDurationMinutes,
    bool IsActive
);

public record UpdateRouteSegmentRequest(
    double DistanceKm,
    int AvgDurationMinutes,
    bool IsActive
);

public record CreateTransportOptionRequest(
    Guid RouteSegmentId,
    TransportMode Mode,
    RouteTier Tier,
    string OperatorName,
    decimal MinCostBdt,
    decimal MaxCostBdt,
    int FrequencyPerDay,
    string? DepartureStation,
    string? ArrivalStation,
    string? BookingLinksJson,
    string? ScheduleNotes
);

public record UpdateTransportOptionRequest(
    TransportMode Mode,
    RouteTier Tier,
    string OperatorName,
    decimal MinCostBdt,
    decimal MaxCostBdt,
    int FrequencyPerDay,
    string? DepartureStation,
    string? ArrivalStation,
    string? BookingLinksJson,
    string? ScheduleNotes
);

public record AdminRouteSegmentDto(
    Guid Id,
    Guid OriginId,
    string OriginName,
    Guid DestinationId,
    string DestinationName,
    double DistanceKm,
    int AvgDurationMinutes,
    bool IsActive,
    List<TransportOptionDto> TransportOptions
);
