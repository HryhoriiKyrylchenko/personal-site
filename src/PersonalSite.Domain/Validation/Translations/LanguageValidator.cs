using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Domain.Validation.Translations;

public class LanguageValidator : AbstractValidator<Language>
{
    public LanguageValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Language ID is required.");

        RuleFor(x => x.Code)
            .NotEmpty().WithMessage("Code is required.")
            .MaximumLength(2).WithMessage("Code must be 2 characters or fewer.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}