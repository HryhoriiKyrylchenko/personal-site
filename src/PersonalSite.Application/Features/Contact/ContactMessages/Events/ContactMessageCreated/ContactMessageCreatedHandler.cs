using PersonalSite.Domain.Entities.Contact;
using PersonalSite.Infrastructure.BackgroundProcessing.BackgroundQueue;

namespace PersonalSite.Application.Features.Contact.ContactMessages.Events.ContactMessageCreated;

public class ContactMessageCreatedHandler : INotificationHandler<ContactMessageCreatedEvent>
{
    private readonly IBackgroundQueue _backgroundQueue;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<ContactMessageCreatedHandler> _logger;
    
    private const int MaxRetries = 3;
    private const string AdminEmail = "contact@hryhoriikyrylchenko.dev";
    
    public ContactMessageCreatedHandler(
        IBackgroundQueue backgroundQueue,
        IEmailSender emailSender,
        ILogger<ContactMessageCreatedHandler> logger)
    {
        _backgroundQueue = backgroundQueue;
        _emailSender = emailSender;
        _logger = logger;
    }
    
    public Task Handle(ContactMessageCreatedEvent notification, CancellationToken cancellationToken)
    {
        var message = notification.Message;

        _backgroundQueue.Enqueue(async token =>
        {
            await SendWithRetryAsync(() => _emailSender.SendAsync(
                    to: AdminEmail,
                    subject: $"New Contact Message: {message.Subject}",
                    body: BuildAdminEmailBody(message),
                    isHtml: true,
                    cancellationToken: token),
                "admin");

            await SendWithRetryAsync(() => _emailSender.SendAsync(
                    to: message.Email,
                    subject: "Thank you for contacting me!",
                    body: BuildUserConfirmationEmailBody(message),
                    isHtml: true,
                    cancellationToken: token),
                "user");
        });

        return Task.CompletedTask;
    }
    
    private static string BuildAdminEmailBody(ContactMessage message) => $"""
        <h2>📬 New Contact Message</h2>
        <table cellpadding="6" cellspacing="0" border="0" style="font-family: Arial; font-size: 14px;">
            <tr><td><strong>Name:</strong></td><td>{message.Name}</td></tr>
            <tr><td><strong>Email:</strong></td><td>{message.Email}</td></tr>
            <tr><td><strong>Subject:</strong></td><td>{message.Subject}</td></tr>
            <tr><td><strong>Message:</strong></td><td>{message.Message}</td></tr>
            <tr><td><strong>IP Address:</strong></td><td>{message.IpAddress}</td></tr>
            <tr><td><strong>User Agent:</strong></td><td>{message.UserAgent}</td></tr>
        </table>
        <br/><p style="color: gray;">Received at {DateTime.UtcNow:yyyy-MM-dd HH:mm:ss} UTC</p>
    """;

    private static string BuildUserConfirmationEmailBody(ContactMessage message) => $"""
        <h2>Thank you, {message.Name}!</h2>
        <p>Your message has been received successfully. I will get back to you as soon as possible.</p>
        <h4>Your Message Summary:</h4>
        <table cellpadding="6" cellspacing="0" border="0" style="font-family: Arial; font-size: 14px;">
            <tr><td><strong>Subject:</strong></td><td>{message.Subject}</td></tr>
            <tr><td><strong>Message:</strong></td><td>{message.Message}</td></tr>
        </table>
        <br/>
        <p>Best regards,<br/>Hryhorii Kyrylchenko</p>
    """;

    private async Task SendWithRetryAsync(
        Func<Task> sendFunc,
        string target)
    {
        for (int attempt = 1; attempt <= MaxRetries; attempt++)
        {
            try
            {
                await sendFunc();
                _logger.LogInformation("Email successfully sent to {Target}.", target);
                return;
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Email attempt {Attempt} to {Target} failed.", attempt, target);

                if (attempt == MaxRetries)
                {
                    _logger.LogError(ex, "Giving up on sending email to {Target} after {MaxRetries} attempts.", target, MaxRetries);
                }
                else
                {
                    await Task.Delay(TimeSpan.FromSeconds(5));
                }
            }
        }
    }
}