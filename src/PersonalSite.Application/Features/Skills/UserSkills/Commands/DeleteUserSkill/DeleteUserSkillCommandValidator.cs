namespace PersonalSite.Application.Features.Skills.UserSkills.Commands.DeleteUserSkill;

public class DeleteUserSkillCommandValidator: AbstractValidator<DeleteUserSkillCommand>
{
    public DeleteUserSkillCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("UserSkill ID is required.");
    }
}