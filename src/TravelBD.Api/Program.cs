using Microsoft.EntityFrameworkCore;
using TravelBD.Application.Interfaces;
using TravelBD.Infrastructure.Persistence;
using TravelBD.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

// Add Controllers and JSON settings
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
    });

// Configure CORS for React client
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Configure Database (Using InMemory by default with automatic fallback/optional PostgreSQL connection)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (!string.IsNullOrEmpty(connectionString))
{
    builder.Services.AddDbContext<TravelDbContext>(options =>
        options.UseNpgsql(connectionString));
}
else
{
    builder.Services.AddDbContext<TravelDbContext>(options =>
        options.UseInMemoryDatabase("TravelBD_Db"));
}

// Register Application & Infrastructure Services
builder.Services.AddScoped<ITravelDataService, TravelDataService>();
builder.Services.AddScoped<IRoutePlannerService, RoutePlannerService>();

var app = builder.Build();

// Ensure database is seeded at launch
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<TravelDbContext>();
    if (dbContext.Database.IsInMemory() || !string.IsNullOrEmpty(connectionString))
    {
        await dbContext.Database.EnsureCreatedAsync();
        await DbSeeder.SeedAsync(dbContext);
    }
}

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthorization();
app.MapControllers();

app.Run();
