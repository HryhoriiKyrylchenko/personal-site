namespace PersonalSite.Application.Features.Contact.ContactMessages.Commands.DeleteContactMessages;

public record DeleteContactMessagesCommand(
    List<Guid> Ids
) : IRequest<Result>;