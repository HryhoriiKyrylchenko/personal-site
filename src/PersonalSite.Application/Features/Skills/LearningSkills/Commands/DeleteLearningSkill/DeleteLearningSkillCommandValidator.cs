namespace PersonalSite.Application.Features.Skills.LearningSkills.Commands.DeleteLearningSkill;

public class DeleteLearningSkillCommandValidator : AbstractValidator<DeleteLearningSkillCommand>
{
    public DeleteLearningSkillCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("LearningSkill ID is required.");
    }
}