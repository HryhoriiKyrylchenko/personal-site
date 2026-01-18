using PersonalSite.Domain.Entities.User;

namespace PersonalSite.Domain.Interfaces.Auth;

public interface IAuthenticationService
{
    Task SignInAsync(User user);
    Task SignOutAsync();
}
