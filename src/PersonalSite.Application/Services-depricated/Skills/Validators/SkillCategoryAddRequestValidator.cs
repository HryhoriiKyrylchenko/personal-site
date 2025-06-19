namespace PersonalSite.Application.Services.Skills.Validators;

public class SkillCategoryAddRequestValidator : AbstractValidator<SkillCategoryAddRequest>
{
    public SkillCategoryAddRequestValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key is required.")
            .MaximumLength(50).WithMessage("Key must be 50 characters or fewer.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo((short)0).WithMessage("DisplayOrder cannot be negative.");
    }
}