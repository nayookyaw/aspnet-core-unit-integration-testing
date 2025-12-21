namespace UserApi.Dtos;

public record UserResponse(
    Guid Id,
    string Username,
    string Email,
    DateTime CreatedAt
);