using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Auth;

namespace PersonalSite.Application.Features.Auth.Authentication.Commands.Logout;

public sealed class LogoutCommandHandler
    : IRequestHandler<LogoutCommand, Result>
{
    private readonly IAuthenticationService _authenticationService;

    public LogoutCommandHandler(IAuthenticationService authenticationService)
    {
        _authenticationService = authenticationService;
    }

    public async Task<Result> Handle(
        LogoutCommand request,
        CancellationToken cancellationToken)
    {
        await _authenticationService.SignOutAsync();
        return Result.Success();
    }
}
