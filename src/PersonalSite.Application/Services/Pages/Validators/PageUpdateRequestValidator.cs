namespace PersonalSite.Application.Services.Pages.Validators;

public class PageUpdateRequestValidator : AbstractValidator<PageUpdateRequest>
{
    public PageUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Page ID is required.");

        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key is required.")
            .MaximumLength(50).WithMessage("Key must be 50 characters or fewer.");
    }
}