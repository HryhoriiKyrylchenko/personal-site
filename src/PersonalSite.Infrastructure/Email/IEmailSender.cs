namespace PersonalSite.Infrastructure.Email;

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string body, bool isHtml, CancellationToken cancellationToken = default);
}