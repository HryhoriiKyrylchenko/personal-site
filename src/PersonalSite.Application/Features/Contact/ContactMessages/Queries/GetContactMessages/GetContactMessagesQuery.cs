using PersonalSite.Application.Features.Contact.ContactMessages.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Contact.ContactMessages.Queries.GetContactMessages;

public record GetContactMessagesQuery(
    int Page = 1,
    int PageSize = 20,
    bool? IsReadFilter = null
) : IRequest<PaginatedResult<ContactMessageDto>>;
