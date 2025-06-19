using PersonalSite.Application.Features.Contact.Common.Dtos;

namespace PersonalSite.Application.Services.Contact;

public interface IContactMessageService : ICrudService<ContactMessageDto, ContactMessageAddRequest, ContactMessageUpdateRequest>
{
    Task<List<ContactMessageDto>> GetUnreadMessagesAsync(CancellationToken cancellationToken = default);
    Task MarkMessageAsReadAsync(Guid id, CancellationToken cancellationToken = default);
    Task MarkMessageAsUnreadAsync(Guid id, CancellationToken cancellationToken = default);
}