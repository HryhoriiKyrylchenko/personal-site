namespace PersonalSite.Domain.Entities.User;

public class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Role { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;
    
    public bool MustChangePassword { get; set; } = true;
}
