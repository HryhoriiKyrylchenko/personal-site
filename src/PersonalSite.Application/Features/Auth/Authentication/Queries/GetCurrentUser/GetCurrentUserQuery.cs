using PersonalSite.Application.Features.Auth.Authentication.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Auth.Authentication.Queries.GetCurrentUser;

public sealed record GetCurrentUserQuery
    : IRequest<Result<CurrentUserDto>>;
