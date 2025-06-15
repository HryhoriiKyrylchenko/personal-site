namespace PersonalSite.Application.Services.Skills.Validators;

public class UserSkillUpdateRequestValidator : AbstractValidator<UserSkillUpdateRequest>
{
    public UserSkillUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("UserSkill ID is required.");

        RuleFor(x => x.Proficiency)
            .InclusiveBetween((short)1, (short)5)
            .WithMessage("Proficiency must be between 1 and 5.");
    }
}