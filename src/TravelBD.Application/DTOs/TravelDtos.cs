using TravelBD.Domain.Entities;
using TravelBD.Domain.Enums;

namespace TravelBD.Application.Common.Models;

public record LocationDto(
    Guid Id,
    string Name,
    string? BanglaName,
    string Slug,
    string Type,
    string District,
    string Division,
    double? Latitude,
    double? Longitude,
    bool IsMajorHub,
    bool IsTouristDestination,
    string? Description,
    string? HeroImageUrl
);

public record TransportOptionDto(
    Guid Id,
    string Mode,
    string Tier,
    string OperatorName,
    decimal MinCostBdt,
    decimal MaxCostBdt,
    int FrequencyPerDay,
    string? DepartureStation,
    string? ArrivalStation,
    string? BookingLinksJson,
    string? ScheduleNotes
);

public record RouteStepDto(
    int StepNumber,
    Guid OriginId,
    string OriginName,
    Guid DestinationId,
    string DestinationName,
    double DistanceKm,
    int AvgDurationMinutes,
    List<TransportOptionDto> TransportOptions
);

public record RoutePlanDto(
    string PlanType, // "Fastest", "Cheapest", "Recommended"
    int TotalDurationMinutes,
    decimal TotalMinCostBdt,
    decimal TotalMaxCostBdt,
    int TotalHops,
    List<RouteStepDto> Steps
);

public record RouteSearchResultDto(
    LocationDto Origin,
    LocationDto Destination,
    List<RoutePlanDto> Plans
);

public record AttractionDto(
    Guid Id,
    string Name,
    string? BanglaName,
    string? Description,
    string? BestTimeToVisit,
    decimal EntryFeeBdt,
    string? ImageUrl,
    string? Category
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
