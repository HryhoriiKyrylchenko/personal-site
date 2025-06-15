namespace PersonalSite.Domain.Validation.Skills;

public class UserSkillValidator : AbstractValidator<UserSkill>
{
    public UserSkillValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("UserSkill ID is required.");

        RuleFor(x => x.SkillId)
            .NotEmpty().WithMessage("SkillId is required.");

        RuleFor(x => x.Proficiency)
            .InclusiveBetween((short)1, (short)5)
            .WithMessage("Proficiency must be between 1 and 5.");

        RuleFor(x => x.CreatedAt)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("CreatedAt cannot be in the future.");

        RuleFor(x => x.UpdatedAt)
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("UpdatedAt cannot be in the future.");
    }
}