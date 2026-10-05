using TravelBD.Application.DTOs;
using TravelBD.Application.Interfaces;
using TravelBD.Domain.Entities;

namespace TravelBD.Application.Managers;

public class AdminRouteManager : IAdminRouteManager
{
    private readonly IAdminRouteRepository _repository;

    public AdminRouteManager(IAdminRouteRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<AdminRouteSegmentDto>> GetAllSegmentsAsync(CancellationToken cancellationToken = default)
    {
        var segments = await _repository.GetAllSegmentsAsync(cancellationToken);
        return segments.Select(MapRouteSegment).ToList();
    }

    public async Task<AdminRouteSegmentDto?> GetSegmentByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var segment = await _repository.GetSegmentByIdAsync(id, cancellationToken);
        return segment == null ? null : MapRouteSegment(segment);
    }

    public async Task<AdminRouteSegmentDto> CreateSegmentAsync(CreateRouteSegmentRequest request, CancellationToken cancellationToken = default)
    {
        if (request.OriginId == request.DestinationId)
            throw new ArgumentException("Origin and Destination cannot be the same location.");

        bool exists = await _repository.SegmentExistsAsync(request.OriginId, request.DestinationId, null, cancellationToken);
        if (exists)
            throw new InvalidOperationException("A route segment between these two locations already exists.");

        var segment = new RouteSegment
        {
            OriginId = request.OriginId,
            DestinationId = request.DestinationId,
            DistanceKm = Math.Max(0.1, request.DistanceKm),
            AvgDurationMinutes = Math.Max(1, request.AvgDurationMinutes),
            IsActive = request.IsActive
        };

        var created = await _repository.CreateSegmentAsync(segment, cancellationToken);
        return MapRouteSegment(created);
    }

    public async Task<AdminRouteSegmentDto?> UpdateSegmentAsync(Guid id, UpdateRouteSegmentRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetSegmentByIdAsync(id, cancellationToken);
        if (existing == null) return null;

        existing.DistanceKm = Math.Max(0.1, request.DistanceKm);
        existing.AvgDurationMinutes = Math.Max(1, request.AvgDurationMinutes);
        existing.IsActive = request.IsActive;

        await _repository.UpdateSegmentAsync(existing, cancellationToken);
        return MapRouteSegment(existing);
    }

    public Task<bool> DeleteSegmentAsync(Guid id, CancellationToken cancellationToken = default)
        => _repository.DeleteSegmentAsync(id, cancellationToken);

    // ── Transport Options ─────────────────────────────────────────────────────
    public async Task<TransportOptionDto> AddTransportOptionAsync(CreateTransportOptionRequest request, CancellationToken cancellationToken = default)
    {
        decimal min = Math.Max(0, request.MinCostBdt);
        decimal max = Math.Max(min, request.MaxCostBdt);

        var option = new TransportOption
        {
            RouteSegmentId = request.RouteSegmentId,
            Mode = request.Mode,
            Tier = request.Tier,
            OperatorName = request.OperatorName.Trim(),
            MinCostBdt = min,
            MaxCostBdt = max,
            FrequencyPerDay = Math.Max(1, request.FrequencyPerDay),
            DepartureStation = request.DepartureStation?.Trim(),
            ArrivalStation = request.ArrivalStation?.Trim(),
            BookingLinksJson = request.BookingLinksJson?.Trim(),
            ScheduleNotes = request.ScheduleNotes?.Trim()
        };

        var created = await _repository.AddTransportOptionAsync(option, cancellationToken);
        return MapTransportOption(created);
    }

    public async Task<TransportOptionDto?> UpdateTransportOptionAsync(Guid id, UpdateTransportOptionRequest request, CancellationToken cancellationToken = default)
    {
        var existing = await _repository.GetTransportOptionByIdAsync(id, cancellationToken);
        if (existing == null) return null;

        decimal min = Math.Max(0, request.MinCostBdt);
        decimal max = Math.Max(min, request.MaxCostBdt);

        existing.Mode = request.Mode;
        existing.Tier = request.Tier;
        existing.OperatorName = request.OperatorName.Trim();
        existing.MinCostBdt = min;
        existing.MaxCostBdt = max;
        existing.FrequencyPerDay = Math.Max(1, request.FrequencyPerDay);
        existing.DepartureStation = request.DepartureStation?.Trim();
        existing.ArrivalStation = request.ArrivalStation?.Trim();
        existing.BookingLinksJson = request.BookingLinksJson?.Trim();
        existing.ScheduleNotes = request.ScheduleNotes?.Trim();

        await _repository.UpdateTransportOptionAsync(existing, cancellationToken);
        return MapTransportOption(existing);
    }

    public Task<bool> DeleteTransportOptionAsync(Guid id, CancellationToken cancellationToken = default)
        => _repository.DeleteTransportOptionAsync(id, cancellationToken);

    // ── Helpers ───────────────────────────────────────────────────────────────
    private static AdminRouteSegmentDto MapRouteSegment(RouteSegment s) => new(
        s.Id,
        s.OriginId,
        s.Origin?.Name ?? string.Empty,
        s.DestinationId,
        s.Destination?.Name ?? string.Empty,
        s.DistanceKm,
        s.AvgDurationMinutes,
        s.IsActive,
        s.TransportOptions.Select(MapTransportOption).ToList()
    );

    private static TransportOptionDto MapTransportOption(TransportOption t) => new(
        t.Id,
        t.Mode.ToString(),
        t.Tier.ToString(),
        t.OperatorName,
        t.MinCostBdt,
        t.MaxCostBdt,
        t.FrequencyPerDay,
        t.DepartureStation,
        t.ArrivalStation,
        t.BookingLinksJson,
        t.ScheduleNotes
    );
}
