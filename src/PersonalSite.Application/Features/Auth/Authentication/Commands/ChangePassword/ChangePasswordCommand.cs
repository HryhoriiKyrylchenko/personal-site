using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Auth.Authentication.Commands.ChangePassword;

public sealed record ChangePasswordCommand(
    string CurrentPassword,
    string NewPassword
) : IRequest<Result>;
