namespace PersonalSite.Application.Services.Translations.Validators;

public class LanguageUpdateRequestValidator : AbstractValidator<LanguageUpdateRequest>
{
    public LanguageUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Translation ID is required.");
        
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Language ID is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(2).WithMessage("Code must be 2 characters or fewer.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}