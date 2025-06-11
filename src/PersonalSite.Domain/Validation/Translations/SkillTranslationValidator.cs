namespace PersonalSite.Domain.Validation.Translations;

public class SkillTranslationValidator : AbstractValidator<SkillTranslation>
{
    public SkillTranslationValidator()
    {
        RuleFor(x => x.SkillId)
            .NotEmpty().WithMessage("SkillId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must be 1000 characters or fewer.");
    }
}