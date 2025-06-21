namespace PersonalSite.Application.Features.Skills.UserSkills.Commands.UpdateUserSkill;

public class UpdateUserSkillCommandValidator : AbstractValidator<UpdateUserSkillCommand>
{
    public UpdateUserSkillCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("UserSkill ID is required.");

        RuleFor(x => x.Proficiency)
            .InclusiveBetween((short)1, (short)5)
            .WithMessage("Proficiency must be between 1 and 5.");
    }
}