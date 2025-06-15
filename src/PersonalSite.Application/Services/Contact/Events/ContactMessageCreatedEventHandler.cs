namespace PersonalSite.Application.Services.Contact.Events;

public class ContactMessageCreatedEventHandler : IEventHandler<ContactMessageCreatedEvent>
{
    private readonly IEmailService _emailService;

    public ContactMessageCreatedEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }

    public async Task HandleAsync(ContactMessageCreatedEvent @event, CancellationToken cancellationToken)
    {
        await _emailService.SendNotificationAsync(@event.Email);
    }
}