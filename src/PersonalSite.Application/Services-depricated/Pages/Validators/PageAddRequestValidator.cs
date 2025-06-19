namespace PersonalSite.Application.Services.Pages.Validators;

public class PageAddRequestValidator : AbstractValidator<PageAddRequest>
{
    public PageAddRequestValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key is required.")
            .MaximumLength(50).WithMessage("Key must be 50 characters or fewer.");
    }
}