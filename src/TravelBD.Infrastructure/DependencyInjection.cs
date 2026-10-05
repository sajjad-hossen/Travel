using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelBD.Application.Interfaces;
using TravelBD.Application.Managers;
using TravelBD.Infrastructure.Persistence;
using TravelBD.Infrastructure.Repositories;

namespace TravelBD.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        // ── Database ──────────────────────────────────────────────────────────
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        if (!string.IsNullOrEmpty(connectionString))
        {
            services.AddDbContext<TravelDbContext>(options =>
                options.UseNpgsql(connectionString));
        }
        else
        {
            services.AddDbContext<TravelDbContext>(options =>
                options.UseInMemoryDatabase("TravelBD_Db"));
        }

        // ── Repositories (Data Access Layer) ──────────────────────────────────
        services.AddScoped<ILocationRepository, LocationRepository>();
        services.AddScoped<IRouteRepository, RouteRepository>();
        services.AddScoped<IAdminLocationRepository, AdminLocationRepository>();
        services.AddScoped<IAdminRouteRepository, AdminRouteRepository>();

        // ── Managers (Business Logic Layer) ───────────────────────────────────
        services.AddScoped<ILocationManager, LocationManager>();
        services.AddScoped<IRouteManager, RouteManager>();
        services.AddScoped<IAdminLocationManager, AdminLocationManager>();
        services.AddScoped<IAdminRouteManager, AdminRouteManager>();

        return services;
    }
}
