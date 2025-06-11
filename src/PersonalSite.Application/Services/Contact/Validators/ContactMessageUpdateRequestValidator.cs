namespace PersonalSite.Application.Services.Contact.Validators;

public class ContactMessageUpdateRequestValidator : AbstractValidator<ContactMessageUpdateRequest>
{
    public ContactMessageUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ContactMessage ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must be 100 characters or fewer.");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email is required.")
            .MaximumLength(150).WithMessage("Email must be 150 characters or fewer.")
            .EmailAddress().WithMessage("Email must be a valid email address.");

        RuleFor(x => x.Subject)
            .MaximumLength(200).WithMessage("Subject must be 200 characters or fewer.");

        RuleFor(x => x.Message)
            .NotEmpty().WithMessage("Message is required.");
    }
}