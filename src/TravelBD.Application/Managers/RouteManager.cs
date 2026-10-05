using TravelBD.Application.DTOs;
using TravelBD.Application.Interfaces;
using TravelBD.Domain.Entities;

namespace TravelBD.Application.Managers;

/// <summary>
/// Handles all route planning business logic including BFS path finding and preference-based sorting.
/// Delegates data access to IRouteRepository.
/// </summary>
public class RouteManager : IRouteManager
{
    private readonly IRouteRepository _routeRepository;

    public RouteManager(IRouteRepository routeRepository)
    {
        _routeRepository = routeRepository;
    }

    public async Task<RouteSearchResultDto?> PlanRouteAsync(
        string originQuery,
        string destinationQuery,
        string? preference = null,
        CancellationToken cancellationToken = default)
    {
        var origin = await _routeRepository.ResolveLocationAsync(originQuery, cancellationToken);
        var destination = await _routeRepository.ResolveLocationAsync(destinationQuery, cancellationToken);

        if (origin == null || destination == null)
            return null;

        if (origin.Id == destination.Id)
        {
            return new RouteSearchResultDto(
                MapLocation(origin),
                MapLocation(destination),
                new List<RoutePlanDto>()
            );
        }

        var allSegments = await _routeRepository.GetAllActiveSegmentsAsync(cancellationToken);

        var paths = FindAllPaths(origin.Id, destination.Id, allSegments, maxDepth: 3);

        var plans = BuildPlans(paths);

        plans = SortByPreference(plans, preference);

        return new RouteSearchResultDto(
            MapLocation(origin),
            MapLocation(destination),
            plans
        );
    }

    // ─── Private Business Logic ──────────────────────────────────────────────

    private static List<RoutePlanDto> BuildPlans(List<List<RouteSegment>> paths)
    {
        var plans = new List<RoutePlanDto>();

        foreach (var path in paths)
        {
            var steps = new List<RouteStepDto>();
            int stepNum = 1;
            int totalDuration = 0;
            decimal totalMinCost = 0;
            decimal totalMaxCost = 0;

            foreach (var segment in path)
            {
                var options = MapTransportOptions(segment.TransportOptions);
                var cheapest = options.OrderBy(o => o.MinCostBdt).FirstOrDefault();

                totalDuration += segment.AvgDurationMinutes;
                totalMinCost += cheapest?.MinCostBdt ?? 0;
                totalMaxCost += cheapest?.MaxCostBdt ?? cheapest?.MinCostBdt ?? 0;

                steps.Add(new RouteStepDto(
                    stepNum++,
                    segment.OriginId,
                    segment.Origin.Name,
                    segment.DestinationId,
                    segment.Destination.Name,
                    segment.DistanceKm,
                    segment.AvgDurationMinutes,
                    options
                ));
            }

            // 45-minute layover penalty for each transit stop
            if (path.Count > 1)
                totalDuration += (path.Count - 1) * 45;

            string planType = path.Count == 1
                ? "Direct Express"
                : $"Via {path[0].Destination.Name} ({path.Count} Legs)";

            plans.Add(new RoutePlanDto(
                planType,
                totalDuration,
                totalMinCost,
                totalMaxCost,
                path.Count,
                steps
            ));
        }

        return plans;
    }

    private static List<RoutePlanDto> SortByPreference(List<RoutePlanDto> plans, string? preference)
    {
        if (preference?.Equals("cheapest", StringComparison.OrdinalIgnoreCase) == true)
            return plans.OrderBy(p => p.TotalMinCostBdt).ThenBy(p => p.TotalDurationMinutes).ToList();

        if (preference?.Equals("fastest", StringComparison.OrdinalIgnoreCase) == true)
            return plans.OrderBy(p => p.TotalDurationMinutes).ThenBy(p => p.TotalMinCostBdt).ToList();

        // Default: recommended — direct first, then fewest hops, then fastest
        return plans.OrderBy(p => p.TotalHops).ThenBy(p => p.TotalDurationMinutes).ToList();
    }

    private static List<List<RouteSegment>> FindAllPaths(
        Guid startId, Guid endId, List<RouteSegment> segments, int maxDepth)
    {
        var results = new List<List<RouteSegment>>();
        Dfs(startId, endId, new List<RouteSegment>(), new HashSet<Guid> { startId }, 0, maxDepth, segments, results);
        return results;
    }

    private static void Dfs(
        Guid currentId,
        Guid targetId,
        List<RouteSegment> currentPath,
        HashSet<Guid> visited,
        int depth,
        int maxDepth,
        List<RouteSegment> allSegments,
        List<List<RouteSegment>> results)
    {
        if (currentId == targetId && currentPath.Count > 0)
        {
            results.Add(new List<RouteSegment>(currentPath));
            return;
        }

        if (depth >= maxDepth) return;

        foreach (var leg in allSegments.Where(s => s.OriginId == currentId))
        {
            if (visited.Contains(leg.DestinationId)) continue;

            visited.Add(leg.DestinationId);
            currentPath.Add(leg);

            Dfs(leg.DestinationId, targetId, currentPath, visited, depth + 1, maxDepth, allSegments, results);

            currentPath.RemoveAt(currentPath.Count - 1);
            visited.Remove(leg.DestinationId);
        }
    }

    private static List<TransportOptionDto> MapTransportOptions(ICollection<TransportOption> options)
        => options.Select(t => new TransportOptionDto(
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
        )).ToList();

    private static LocationDto MapLocation(Location l)
        => new(
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
