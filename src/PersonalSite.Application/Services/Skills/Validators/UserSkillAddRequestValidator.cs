namespace PersonalSite.Application.Services.Skills.Validators;

public class UserSkillAddRequestValidator : AbstractValidator<UserSkillAddRequest>
{
    public UserSkillAddRequestValidator()
    {
        RuleFor(x => x.SkillId)
            .NotEmpty().WithMessage("SkillId is required.");

        RuleFor(x => x.Proficiency)
            .InclusiveBetween((short)1, (short)5)
            .WithMessage("Proficiency must be between 1 and 5.");

    }
}