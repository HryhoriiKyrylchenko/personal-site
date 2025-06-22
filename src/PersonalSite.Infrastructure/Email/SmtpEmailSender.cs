using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Options;

namespace PersonalSite.Infrastructure.Email;

public class SmtpEmailSender : IEmailSender
{
    private readonly SmtpSettings _settings;
    private readonly ILogger<SmtpEmailSender> _logger;

    public SmtpEmailSender(IOptions<SmtpSettings> options, ILogger<SmtpEmailSender> logger)
    {
        _settings = options.Value;
        _logger = logger;
    }
    
    public async Task SendAsync(string to, string subject, string body, bool isHtml, CancellationToken cancellationToken = default)
    {
        try
        {
            using var client = new SmtpClient(_settings.Host, _settings.Port);
            client.EnableSsl = _settings.EnableSsl;
            client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);

            using var message = new MailMessage();
            message.From = new MailAddress(_settings.From);
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = isHtml;

            message.To.Add(to);

            await client.SendMailAsync(message, cancellationToken);

            _logger.LogInformation("Email sent to {Recipient}", to);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to send email to {Recipient}", to);
            throw;
        }
    }
}