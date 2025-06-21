namespace PersonalSite.Application.Features.Skills.LearningSkills.Commands.CreateLearningSkill;

public class CreateLearningSkillCommandValidator : AbstractValidator<CreateLearningSkillCommand>
{
    public CreateLearningSkillCommandValidator()
    {
        RuleFor(x => x.SkillId)
            .NotEmpty().WithMessage("SkillId is required.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo((short)0).WithMessage("DisplayOrder cannot be negative.");
    }
}