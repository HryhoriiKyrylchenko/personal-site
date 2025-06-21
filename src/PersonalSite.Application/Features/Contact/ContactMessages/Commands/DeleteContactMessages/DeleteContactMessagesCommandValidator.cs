namespace PersonalSite.Application.Features.Contact.ContactMessages.Commands.DeleteContactMessages;

public class DeleteContactMessagesCommandValidator : AbstractValidator<DeleteContactMessagesCommand>
{
    public DeleteContactMessagesCommandValidator()
    {
        RuleFor(x => x.Ids)
            .NotEmpty().WithMessage("At least one message Id must be provided.");
    }
}