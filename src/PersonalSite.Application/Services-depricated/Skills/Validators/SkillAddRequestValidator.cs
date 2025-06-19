namespace PersonalSite.Application.Services.Skills.Validators;

public class SkillAddRequestValidator : AbstractValidator<SkillAddRequest>
{
    public SkillAddRequestValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");

        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key is required.")
            .MaximumLength(50).WithMessage("Key must be 50 characters or fewer.");
    }
}