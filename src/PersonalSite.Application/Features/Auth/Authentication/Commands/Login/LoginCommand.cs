using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Auth.Authentication.Commands.Login;

public record LoginCommand(
    string Username,
    string Password
) : IRequest<Result<LoginResponse>>;