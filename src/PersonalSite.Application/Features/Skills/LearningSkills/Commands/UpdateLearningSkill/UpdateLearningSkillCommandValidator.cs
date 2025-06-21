namespace PersonalSite.Application.Features.Skills.LearningSkills.Commands.UpdateLearningSkill;

public class UpdateLearningSkillCommandValidator : AbstractValidator<UpdateLearningSkillCommand>
{
    public UpdateLearningSkillCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("LearningSkill ID is required.");

        RuleFor(x => x.LearningStatus)
            .IsInEnum().WithMessage("LearningStatus must be a valid enum value.");

        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo((short)0).WithMessage("DisplayOrder cannot be negative.");
    }
}