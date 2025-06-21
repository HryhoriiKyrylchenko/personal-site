namespace PersonalSite.Application.Features.Skills.UserSkills.Commands.CreateUserSkill;

public class CreateUserSkillCommandValidator : AbstractValidator<CreateUserSkillCommand>
{
    public CreateUserSkillCommandValidator()
    {
        RuleFor(x => x.SkillId)
            .NotEmpty().WithMessage("SkillId is required.");

        RuleFor(x => x.Proficiency)
            .InclusiveBetween((short)1, (short)5)
            .WithMessage("Proficiency must be between 1 and 5.");

    }
}