using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using TravelBD.Infrastructure;
using TravelBD.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// ── Controllers ───────────────────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        options.JsonSerializerOptions.Converters.Add(new System.Text.Json.Serialization.JsonStringEnumConverter());
    });

// ── CORS ──────────────────────────────────────────────────────────────────────
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
});

// ── Infrastructure (DB + Repositories + Managers) ────────────────────────────
builder.Services.AddInfrastructure(builder.Configuration);

// ── Authentication (JWT) ──────────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("Jwt");
var jwtKey = jwtSection["Key"] ?? "travelbd-dev-jwt-signing-key-change-me-2026";
var jwtIssuer = jwtSection["Issuer"] ?? "TravelBD";
var jwtAudience = jwtSection["Audience"] ?? "TravelBD";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization();

// ─────────────────────────────────────────────────────────────────────────────
var app = builder.Build();

// ── Seed Database ─────────────────────────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TravelDbContext>();
    await db.Database.EnsureCreatedAsync();

    // Ensure newly added columns exist in existing PostgreSQL schema
    try
    {
        await db.Database.ExecuteSqlRawAsync(
            "ALTER TABLE \"Accommodations\" ADD COLUMN IF NOT EXISTS \"ImageUrl\" text;"
        );
    }
    catch
    {
        // Ignore if column already exists or table doesn't exist yet
    }

    await DbSeeder.SeedAsync(db);
}

// ── Middleware Pipeline ───────────────────────────────────────────────────────
app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
