using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Auth.Authentication.Commands.Logout;

public record LogoutCommand(): IRequest<Result>;
