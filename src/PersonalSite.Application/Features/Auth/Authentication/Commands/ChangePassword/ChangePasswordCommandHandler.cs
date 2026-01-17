using Microsoft.AspNetCore.Identity;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.User;
using PersonalSite.Domain.Interfaces.Auth;
using PersonalSite.Domain.Interfaces.Repositories.User;

namespace PersonalSite.Application.Features.Auth.Authentication.Commands.ChangePassword;

public sealed class ChangePasswordCommandHandler
    : IRequestHandler<ChangePasswordCommand, Result>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IUserRepository _userRepository;
    private readonly IAuthenticationService _authenticationService;
    private readonly ICurrentUserService _currentUser;

    public ChangePasswordCommandHandler(
        IUnitOfWork unitOfWork, 
        IUserRepository userRepository,
        IAuthenticationService authenticationService,
        ICurrentUserService currentUser)
    {
        _unitOfWork = unitOfWork;
        _userRepository = userRepository;
        _authenticationService = authenticationService;
        _currentUser = currentUser;
    }

    public async Task<Result> Handle(
        ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        if (!_currentUser.IsAuthenticated)
            return Result.Failure("Unauthorized");
        
        var user = await _userRepository.GetById(_currentUser.UserId, cancellationToken);

        if (user is null)
            return Result.Failure("User not found");

        var hasher = new PasswordHasher<User>();

        var verify = hasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.CurrentPassword);

        if (verify == PasswordVerificationResult.Failed)
            return Result.Failure("Invalid current password");

        user.PasswordHash = hasher.HashPassword(user, request.NewPassword);
        user.MustChangePassword = false;

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        await _authenticationService.SignInAsync(user);

        return Result.Success();
    }
}
