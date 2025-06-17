namespace PersonalSite.Application.Features.Contact.ContactMessages.Events.ContactMessageCreated;

public class ContactMessageCreatedEvent : INotification
{
    public ContactMessageCreatedEvent(ContactMessage message)
    {
        Message = message;
    }

    public ContactMessage Message { get; }
}