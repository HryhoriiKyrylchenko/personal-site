using PersonalSite.Application.Features.Contact.ContactMessages.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Contact.ContactMessages.Queries.GetContactMessageById;

public record GetContactMessageByIdQuery(Guid Id) : IRequest<Result<ContactMessageDto>>;