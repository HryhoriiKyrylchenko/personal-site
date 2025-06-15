namespace PersonalSite.Application.Services.Translations.Validators;

public class SkillTranslationUpdateRequestValidator : AbstractValidator<SkillTranslationUpdateRequest>
{
    public SkillTranslationUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Translation ID is required.");
        
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must be 1000 characters or fewer.");
    }
}