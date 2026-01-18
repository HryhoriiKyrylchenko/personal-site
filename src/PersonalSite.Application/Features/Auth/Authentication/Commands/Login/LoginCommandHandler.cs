using Microsoft.AspNetCore.Identity;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.User;
using PersonalSite.Domain.Interfaces.Auth;
using PersonalSite.Infrastructure.Persistence;

namespace PersonalSite.Application.Features.Auth.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
{
    private readonly ApplicationDbContext _context;
    private readonly IAuthenticationService _authService;

    public LoginCommandHandler(
        ApplicationDbContext context,
        IAuthenticationService authService)
    {
        _context = context;
        _authService = authService;
    }

    public async Task<Result<LoginResponse>> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u =>
                    u.Username == request.Username &&
                    u.IsActive,
                cancellationToken);

        if (user == null)
            return Result<LoginResponse>.Failure("Invalid credentials");

        var hasher = new PasswordHasher<User>();
        var result = hasher.VerifyHashedPassword(
            user,
            user.PasswordHash,
            request.Password);

        if (result == PasswordVerificationResult.Failed)
            return Result<LoginResponse>.Failure("Invalid credentials");

        await _authService.SignInAsync(user);

        return Result<LoginResponse>.Success(
            new LoginResponse(user.MustChangePassword));
    }
}