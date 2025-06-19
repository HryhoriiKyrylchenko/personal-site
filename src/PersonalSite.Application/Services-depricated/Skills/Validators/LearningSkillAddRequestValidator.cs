namespace PersonalSite.Application.Services.Skills.Validators;

public class LearningSkillAddRequestValidator : AbstractValidator<LearningSkillAddRequest>
{
    public LearningSkillAddRequestValidator()
    {
        RuleFor(x => x.SkillId)
            .NotEmpty().WithMessage("SkillId is required.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo((short)0).WithMessage("DisplayOrder cannot be negative.");
    }
}