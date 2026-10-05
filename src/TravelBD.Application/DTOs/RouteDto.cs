namespace TravelBD.Application.DTOs;

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
    string PlanType,             // "Direct Express", "Fastest", "Cheapest"
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
