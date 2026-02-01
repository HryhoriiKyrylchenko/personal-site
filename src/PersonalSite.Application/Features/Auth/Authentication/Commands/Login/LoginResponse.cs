namespace PersonalSite.Application.Features.Auth.Authentication.Commands.Login;

public sealed record LoginResponse(
    bool MustChangePassword
);
