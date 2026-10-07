using Microsoft.EntityFrameworkCore;
using TravelBD.Domain.Entities;

namespace TravelBD.Infrastructure.Persistence;

public class TravelDbContext : DbContext
{
    public TravelDbContext(DbContextOptions<TravelDbContext> options) : base(options)
    {
    }

    public DbSet<Location> Locations => Set<Location>();
    public DbSet<RouteSegment> RouteSegments => Set<RouteSegment>();
    public DbSet<TransportOption> TransportOptions => Set<TransportOption>();
    public DbSet<Attraction> Attractions => Set<Attraction>();
    public DbSet<Accommodation> Accommodations => Set<Accommodation>();
    public DbSet<DestinationAdvisory> DestinationAdvisories => Set<DestinationAdvisory>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Feedback> Feedbacks => Set<Feedback>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Location>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Slug).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(120).IsRequired();
            entity.Property(e => e.Slug).HasMaxLength(140).IsRequired();
        });

        modelBuilder.Entity<RouteSegment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Origin)
                .WithMany(l => l.OutgoingRoutes)
                .HasForeignKey(e => e.OriginId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(e => e.Destination)
                .WithMany(l => l.IncomingRoutes)
                .HasForeignKey(e => e.DestinationId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => new { e.OriginId, e.DestinationId }).IsUnique();
        });

        modelBuilder.Entity<TransportOption>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.RouteSegment)
                .WithMany(s => s.TransportOptions)
                .HasForeignKey(e => e.RouteSegmentId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Attraction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Location)
                .WithMany(l => l.Attractions)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Accommodation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Location)
                .WithMany(l => l.Accommodations)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<DestinationAdvisory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Location)
                .WithMany(l => l.Advisories)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Email).IsUnique();
            entity.Property(e => e.Name).HasMaxLength(120).IsRequired();
            entity.Property(e => e.Email).HasMaxLength(180).IsRequired();
            entity.Property(e => e.PasswordHash).IsRequired();
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Comment).HasMaxLength(2000).IsRequired();

            entity.HasOne(e => e.User)
                .WithMany(u => u.Feedbacks)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Location)
                .WithMany(l => l.Feedbacks)
                .HasForeignKey(e => e.LocationId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Attraction)
                .WithMany(a => a.Feedbacks)
                .HasForeignKey(e => e.AttractionId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
