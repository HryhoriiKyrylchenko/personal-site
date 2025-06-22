using PersonalSite.Domain.Entities.Contact;

namespace PersonalSite.Application.Features.Contact.ContactMessages.Events.ContactMessageCreated;

public record ContactMessageCreatedEvent(ContactMessage Message) : INotification;