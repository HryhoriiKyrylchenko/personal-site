using PersonalSite.Application.Features.Contact.ContactMessages.Dtos;
using PersonalSite.Domain.Entities.Contact;

namespace PersonalSite.Application.Features.Contact.ContactMessages.Mappers;

public class ContactMessageMapper : IMapper<ContactMessage, ContactMessageDto>
{
    public ContactMessageDto MapToDto(ContactMessage entity)
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
    
    public List<ContactMessageDto> MapToDtoList(IEnumerable<ContactMessage> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}