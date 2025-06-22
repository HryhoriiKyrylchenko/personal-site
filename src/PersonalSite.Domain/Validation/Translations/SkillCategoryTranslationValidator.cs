using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Domain.Validation.Translations;

public class SkillCategoryTranslationValidator : AbstractValidator<SkillCategoryTranslation>
{
    public SkillCategoryTranslationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Translation ID is required.");
        
        RuleFor(x => x.SkillCategoryId)
            .NotEmpty().WithMessage("SkillCategoryId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must be 100 characters or fewer.");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Description must be 255 characters or fewer.");
    }
}