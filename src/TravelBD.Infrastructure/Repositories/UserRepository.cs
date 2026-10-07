using Microsoft.EntityFrameworkCore;
using TravelBD.Application.Interfaces;
using TravelBD.Domain.Entities;
using TravelBD.Infrastructure.Persistence;

namespace TravelBD.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TravelDbContext _context;

    public UserRepository(TravelDbContext context)
    {
        _context = context;
    }

    public Task<User?> GetByEmailAsync(string email, CancellationToken ct) =>
        _context.Users.FirstOrDefaultAsync(u => u.Email == email, ct);

    public Task<User?> GetByIdAsync(Guid id, CancellationToken ct) =>
        _context.Users.FirstOrDefaultAsync(u => u.Id == id, ct);

    public async Task AddAsync(User user, CancellationToken ct) =>
        await _context.Users.AddAsync(user, ct);

    public Task SaveChangesAsync(CancellationToken ct) =>
        _context.SaveChangesAsync(ct);
}
