using Microsoft.EntityFrameworkCore;
using UserApi.Data;
using UserApi.Dtos;
using UserApi.Models;

namespace UserApi.Services;

public class UserService
{
    private readonly AppDbContext _db;
    public UserService(AppDbContext db) => _db = db;
    public async Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct = default)
    {
        var email = request.Email.Trim().ToLowerInvariant();
        Boolean isExit = await _db.Users.AnyAsync(user => user.Email == email, ct);
        if (isExit)
        {
            throw new InvalidOperationException("Email already exists!");
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
}