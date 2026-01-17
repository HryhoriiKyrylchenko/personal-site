using PersonalSite.Application.Features.Auth.Authentication.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Auth;

namespace PersonalSite.Application.Features.Auth.Authentication.Queries.GetCurrentUser;

public sealed class GetCurrentUserQueryHandler
    : IRequestHandler<GetCurrentUserQuery, Result<CurrentUserDto>>
{
    private readonly ICurrentUserService _currentUser;

    public GetCurrentUserQueryHandler(ICurrentUserService currentUser)
    {
        _currentUser = currentUser;
    }

    public Task<Result<CurrentUserDto>> Handle(
        GetCurrentUserQuery request,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated)
            return Task.FromResult(
                Result<CurrentUserDto>.Failure("Unauthorized"));

        var dto = new CurrentUserDto(
            _currentUser.UserId,
            _currentUser.Username,
            "Admin",
            _currentUser.MustChangePassword);

        return Task.FromResult(
            Result<CurrentUserDto>.Success(dto));
    }
}
