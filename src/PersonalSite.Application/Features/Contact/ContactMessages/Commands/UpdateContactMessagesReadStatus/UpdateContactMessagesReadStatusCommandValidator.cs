namespace PersonalSite.Application.Features.Contact.ContactMessages.Commands.UpdateContactMessagesReadStatus;

public class UpdateContactMessagesReadStatusCommandValidator : AbstractValidator<UpdateContactMessagesReadStatusCommand>
{
    public UpdateContactMessagesReadStatusCommandValidator()
    {
        RuleFor(x => x.Ids)
            .NotEmpty().WithMessage("At least one message Id must be provided.");
    }
}