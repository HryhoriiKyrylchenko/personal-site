using PersonalSite.Application.Features.Contact.ContactMessages.Commands.SendContactMessage;
using PersonalSite.Application.Features.Contact.ContactMessages.Commands.UpdateContactMessagesReadStatus;
using PersonalSite.Application.Features.Contact.ContactMessages.Dtos;
using PersonalSite.Domain.Entities.Contact;

namespace PersonalSite.Application.Tests.Fixtures.TestDataFactories;

public class ContactTestDataFactory
{
    public static ContactMessage CreateContactMessage(
        Guid? id = null,
        string? name = "John Doe",
        string? email = "john@example.com",
        string? subject = "Test Subject",
        string? message = "Test message content.",
        string? userAgent = "Mozilla/5.0",
        DateTime? createdAt = null,
        bool isRead = false)
    {
        return new ContactMessage
        {
            Id = id ?? Guid.NewGuid(),
            Name = name!,
            Email = email!,
            Subject = subject!,
            Message = message!,
            UserAgent = userAgent!,
            CreatedAt = createdAt ?? DateTime.UtcNow,
            IsRead = isRead
        };
    }

    public static ContactMessageDto MapToDto(ContactMessage entity)
    {
        return new ContactMessageDto
        {
            Id = entity.Id,
            Name = entity.Name,
            Email = entity.Email,
            Subject = entity.Subject,
            Message = entity.Message,
            CreatedAt = entity.CreatedAt,
            IsRead = entity.IsRead
        };
    }

    public static SendContactMessageCommand CreateSendContactMessageCommand(
        string name = "John", 
        string email = "Doe", 
        string subject = "Subject", 
        string message = "Message")
    {
        return new SendContactMessageCommand(name, email, subject, message)
        {
            UserAgent = "TestAgent"       
        };
    }

    public static UpdateContactMessagesReadStatusCommand CreateUpdateContactMessagesReadStatusCommand(
        List<Guid> ids, bool isRead = true)
    {
        return new UpdateContactMessagesReadStatusCommand(ids, isRead);
    }
}