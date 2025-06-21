namespace PersonalSite.Application.Features.Contact.ContactMessages.Mappers;

public static class ContactMessageMapper
{
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
    
    public static List<ContactMessageDto> MapToDtoList(IEnumerable<ContactMessage> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}