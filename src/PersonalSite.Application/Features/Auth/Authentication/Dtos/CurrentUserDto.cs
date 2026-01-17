namespace PersonalSite.Application.Features.Auth.Authentication.Dtos;

public sealed record CurrentUserDto(
    Guid Id,
    string Username,
    string Role,
    bool MustChangePassword
);
