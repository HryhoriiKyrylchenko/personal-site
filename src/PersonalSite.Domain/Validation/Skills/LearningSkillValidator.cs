namespace PersonalSite.Domain.Validation.Skills;

public class LearningSkillValidator : AbstractValidator<LearningSkill>
{
    public LearningSkillValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("LearningSkill ID is required.");

        RuleFor(x => x.SkillId)
            .NotEmpty().WithMessage("SkillId is required.");

        RuleFor(x => x.LearningStatus)
            .IsInEnum().WithMessage("LearningStatus must be a valid enum value.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo((short)0).WithMessage("DisplayOrder cannot be negative.");
    }
}