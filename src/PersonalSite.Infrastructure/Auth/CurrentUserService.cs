using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using PersonalSite.Domain.Interfaces.Auth;

namespace PersonalSite.Infrastructure.Auth;

public sealed class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CurrentUserService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? User =>
        _httpContextAccessor.HttpContext?.User;

    public bool IsAuthenticated =>
        User?.Identity?.IsAuthenticated == true;

    public Guid UserId =>
        Guid.Parse(User!.FindFirstValue(ClaimTypes.NameIdentifier)!);

    public string Username =>
        User!.Identity!.Name!;

    public bool MustChangePassword =>
        bool.Parse(User!.FindFirstValue("must_change_password")!);
}