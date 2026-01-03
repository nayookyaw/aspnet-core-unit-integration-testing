using Microsoft.EntityFrameworkCore;
using UserApi.Consts;
using UserApi.Data;
using UserApi.Dtos;
using UserApi.Models;

namespace UserApi.Services;

public class UserService : IUserService
{
    private readonly AppDbContext _db;
    public UserService(AppDbContext db) => _db = db;
    public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        Boolean isExit = await _db.Users.AnyAsync(user => user.Email == email, ct);
        if (isExit)
        {
            throw new InvalidOperationException(Message.DuplicateEmail);
        }
        User newUser = new User
        {
            Username = request.Username.Trim(),
            Email = email,
            CreatedAt = DateTime.UtcNow
        };
        _db.Users.Add(newUser);
        await _db.SaveChangesAsync(ct);
        return new UserResponse(newUser.Id, newUser.Username, newUser.Email, newUser.CreatedAt);
    }

    public async Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        User? user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
        return user is null ? null : new UserResponse(user.Id, user.Username, user.Email, user.CreatedAt);
    }

    public async Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.Users.AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(user => new UserResponse(user.Id, user.Username, user.Email, user.CreatedAt))
            .ToListAsync(ct);
    }
}