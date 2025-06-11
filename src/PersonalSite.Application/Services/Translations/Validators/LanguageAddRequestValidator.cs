namespace PersonalSite.Application.Services.Translations.Validators;

public class LanguageAddRequestValidator : AbstractValidator<LanguageAddRequest>
{
    public LanguageAddRequestValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(2).WithMessage("Code must be 2 characters or fewer.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}