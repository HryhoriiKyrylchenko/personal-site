namespace PersonalSite.Application.Services.Translations.Validators;

public class SkillTranslationAddRequestValidator : AbstractValidator<SkillTranslationAddRequest>
{
    public SkillTranslationAddRequestValidator()
    {
        RuleFor(x => x.SkillId)
            .NotEmpty().WithMessage("SkillId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must be 1000 characters or fewer.");
    }
}