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
        string? ipAddress = "127.0.0.1",
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
            IpAddress = ipAddress!,
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
}