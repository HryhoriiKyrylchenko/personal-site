using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Contact.ContactMessages.Commands.UpdateContactMessagesReadStatus;

public record UpdateContactMessagesReadStatusCommand(
    List<Guid> Ids,
    bool IsRead
) : IRequest<Result>;