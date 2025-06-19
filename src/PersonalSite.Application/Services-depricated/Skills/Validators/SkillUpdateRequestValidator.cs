namespace PersonalSite.Application.Services.Skills.Validators;

public class SkillUpdateRequestValidator : AbstractValidator<SkillUpdateRequest>
{
    public SkillUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Skill ID is required.");

        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");

        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key is required.")
            .MaximumLength(50).WithMessage("Key must be 50 characters or fewer.");
    }
}