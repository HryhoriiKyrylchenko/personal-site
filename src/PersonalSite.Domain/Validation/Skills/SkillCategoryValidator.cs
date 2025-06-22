using PersonalSite.Domain.Entities.Skills;

namespace PersonalSite.Domain.Validation.Skills;

public class SkillCategoryValidator : AbstractValidator<SkillCategory>
{
    public SkillCategoryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("SkillCategory ID is required.");

        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key is required.")
            .MaximumLength(50).WithMessage("Key must be 50 characters or fewer.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo((short)0).WithMessage("DisplayOrder cannot be negative.");
    }
}