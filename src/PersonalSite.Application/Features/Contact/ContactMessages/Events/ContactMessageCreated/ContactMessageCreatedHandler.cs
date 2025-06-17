namespace PersonalSite.Application.Features.Contact.ContactMessages.Events.ContactMessageCreated;

public class ContactMessageCreatedHandler : INotificationHandler<ContactMessageCreatedEvent>
{
    private readonly IBackgroundQueue _backgroundQueue;
    private readonly IEmailSender _emailSender;
    private readonly ILogger<ContactMessageCreatedHandler> _logger;
    
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

        var emailBody = $"""
                             <b>New Contact Message</b><br/>
                             <b>Name:</b> {message.Name}<br/>
                             <b>Email:</b> {message.Email}<br/>
                             <b>Subject:</b> {message.Subject}<br/>
                             <b>Message:</b> {message.Message}<br/>
                             <b>IP:</b> {message.IpAddress}<br/>
                             <b>User Agent:</b> {message.UserAgent}<br/>
                         """;

        _backgroundQueue.Enqueue(async token =>
        {
            const int maxRetries = 3;
            for (int attempt = 1; attempt <= maxRetries; attempt++)
            {
                try
                {
                    await _emailSender.SendAsync(
                        to: "contact@hryhoriikyrylchenko.dev",
                        subject: $"New Contact Message: {message.Subject}",
                        body: emailBody,
                        isHtml: true,
                        token);

                    _logger.LogInformation("Contact email sent.");
                    return;
                }
                catch (Exception ex)
                {
                    _logger.LogWarning(ex, "Email send attempt {Attempt} failed.", attempt);

                    if (attempt == maxRetries)
                    {
                        _logger.LogError(ex, "Giving up on sending email after {MaxRetries} attempts.", maxRetries);
                    }
                    else
                    {
                        await Task.Delay(TimeSpan.FromSeconds(5), token);
                    }
                }
            }
        });
        return Task.CompletedTask;
    }
}