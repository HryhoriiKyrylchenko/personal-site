namespace PersonalSite.Domain.Interfaces.Auth;

public interface ICurrentUserService
{
    Guid UserId { get; }
    string Username { get; }
    bool IsAuthenticated { get; }
    bool MustChangePassword { get; }
}
