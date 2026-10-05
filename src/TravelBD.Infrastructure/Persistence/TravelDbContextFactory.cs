using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace TravelBD.Infrastructure.Persistence;

public class TravelDbContextFactory : IDesignTimeDbContextFactory<TravelDbContext>
{
    public TravelDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TravelDbContext>();
        
        // Design-time connection string for generating PostgreSQL migrations
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=travelbd;Username=postgres;Password=sajjad");

        return new TravelDbContext(optionsBuilder.Options);
    }
}
