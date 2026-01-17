using Microsoft.AspNetCore.Identity;
using PersonalSite.Domain.Entities.User;

namespace PersonalSite.Infrastructure.Persistence.Seed;

public static class AdminSeeder
{
    public static void Seed(ApplicationDbContext context)
    {
        if (context.Users.Any())
            return;

        var admin = new User
        {
            Id = Guid.NewGuid(),
            Username = "admin",
            Role = "Admin"
        };

        var hasher = new PasswordHasher<User>();
        admin.PasswordHash = hasher.HashPassword(admin, "Admin123!");
        admin.MustChangePassword = true;

        context.Users.Add(admin);
        context.SaveChanges();
    }
}