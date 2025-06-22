using PersonalSite.Application.Features.Contact.ContactMessages.Dtos;

namespace PersonalSite.Application.Features.Contact.ContactMessages.Queries.GetContactMessageById;

public record GetContactMessageByIdQuery(Guid Id) : IRequest<Result<ContactMessageDto>>;