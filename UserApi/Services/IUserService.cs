using UserApi.Dtos;

namespace UserApi.Services;

public interface IUserService
{
    Task<UserResponse> CreateAsync(CreateUserRequest request, CancellationToken ct = default);
    Task<UserResponse?> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<IReadOnlyList<UserResponse>> GetAllAsync(CancellationToken ct = default);
}