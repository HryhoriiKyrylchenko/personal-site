namespace PersonalSite.Infrastructure.Email;

public interface IEmailService
{
    Task SendNotificationAsync(string eventEmail);
}