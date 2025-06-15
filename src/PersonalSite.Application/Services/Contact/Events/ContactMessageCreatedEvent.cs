namespace PersonalSite.Application.Services.Contact.Events;

public class ContactMessageCreatedEvent : IEvent
{
    public Guid ContactMessageId { get; }
    public string Email { get; }

    public ContactMessageCreatedEvent(Guid contactMessageId, string email)
    {
        ContactMessageId = contactMessageId;
        Email = email;
    }
}