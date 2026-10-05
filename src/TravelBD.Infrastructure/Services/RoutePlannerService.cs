using Microsoft.EntityFrameworkCore;
using TravelBD.Application.Common.Models;
using TravelBD.Application.Interfaces;
using TravelBD.Domain.Entities;
using TravelBD.Infrastructure.Persistence;

namespace TravelBD.Infrastructure.Services;

public class RoutePlannerService : IRoutePlannerService
{
    private readonly TravelDbContext _context;

    public RoutePlannerService(TravelDbContext context)
    {
        _context = context;
    }

    public async Task<RouteSearchResultDto?> PlanRouteAsync(
        string originQuery,
        string destinationQuery,
        string? preference = null,
        CancellationToken cancellationToken = default)
    {
        var origin = await ResolveLocationAsync(originQuery, cancellationToken);
        var destination = await ResolveLocationAsync(destinationQuery, cancellationToken);

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

        // Load all active route segments with transport options
        var allSegments = await _context.RouteSegments
            .Include(s => s.Origin)
            .Include(s => s.Destination)
            .Include(s => s.TransportOptions)
            .Where(s => s.IsActive)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

        // Find candidate paths using BFS graph exploration up to 3 hops (Origin -> Transit1 -> Transit2 -> Destination)
        var paths = FindAllPaths(origin.Id, destination.Id, allSegments, maxDepth: 3);

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
                var options = segment.TransportOptions.Select(t => new TransportOptionDto(
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

                var cheapest = options.OrderBy(o => o.MinCostBdt).FirstOrDefault();
                var fastestDuration = segment.AvgDurationMinutes;

                totalDuration += fastestDuration;
                totalMinCost += cheapest?.MinCostBdt ?? 0;
                totalMaxCost += cheapest?.MaxCostBdt ?? (cheapest?.MinCostBdt ?? 0);

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

            // Transit waiting penalty for multi-hop
            if (path.Count > 1)
            {
                totalDuration += (path.Count - 1) * 45; // 45 min layover estimation per transit
            }

            string planType = path.Count == 1 ? "Direct Express" : $"Via {path[0].Destination.Name} ({path.Count} Legs)";

            plans.Add(new RoutePlanDto(
                planType,
                totalDuration,
                totalMinCost,
                totalMaxCost,
                path.Count,
                steps
            ));
        }

        // Sort plans according to preference or default
        if (preference?.Equals("cheapest", StringComparison.OrdinalIgnoreCase) == true)
        {
            plans = plans.OrderBy(p => p.TotalMinCostBdt).ThenBy(p => p.TotalDurationMinutes).ToList();
        }
        else if (preference?.Equals("fastest", StringComparison.OrdinalIgnoreCase) == true)
        {
            plans = plans.OrderBy(p => p.TotalDurationMinutes).ThenBy(p => p.TotalMinCostBdt).ToList();
        }
        else
        {
            // Recommended: Direct first, then fastest
            plans = plans.OrderBy(p => p.TotalHops).ThenBy(p => p.TotalDurationMinutes).ToList();
        }

        return new RouteSearchResultDto(
            MapLocation(origin),
            MapLocation(destination),
            plans
        );
    }

    private List<List<RouteSegment>> FindAllPaths(Guid startId, Guid endId, List<RouteSegment> segments, int maxDepth)
    {
        var results = new List<List<RouteSegment>>();
        var currentPath = new List<RouteSegment>();
        var visitedNodes = new HashSet<Guid> { startId };

        Dfs(startId, endId, currentPath, visitedNodes, 0, maxDepth, segments, results);

        return results;
    }

    private void Dfs(
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

        var availableLegs = allSegments.Where(s => s.OriginId == currentId).ToList();

        foreach (var leg in availableLegs)
        {
            if (!visited.Contains(leg.DestinationId))
            {
                visited.Add(leg.DestinationId);
                currentPath.Add(leg);

                Dfs(leg.DestinationId, targetId, currentPath, visited, depth + 1, maxDepth, allSegments, results);

                currentPath.RemoveAt(currentPath.Count - 1);
                visited.Remove(leg.DestinationId);
            }
        }
    }

    private async Task<Location?> ResolveLocationAsync(string query, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(query)) return null;

        var q = query.Trim().ToLower();

        if (Guid.TryParse(query, out var id))
        {
            return await _context.Locations.FindAsync(new object[] { id }, cancellationToken);
        }

        return await _context.Locations.FirstOrDefaultAsync(
            l => l.Slug.ToLower() == q ||
                 l.Name.ToLower() == q ||
                 (l.BanglaName != null && l.BanglaName == q) ||
                 l.District.ToLower() == q,
            cancellationToken);
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
