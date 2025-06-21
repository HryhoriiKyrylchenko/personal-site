namespace PersonalSite.Application.Features.Skills.Skills.Commands.CreateSkill;

public class CreateSkillCommandValidator : AbstractValidator<CreateSkillCommand>
{
    public CreateSkillCommandValidator()
    {
        RuleFor(x => x.CategoryId)
            .NotEmpty().WithMessage("CategoryId is required.");

        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key is required.")
            .MaximumLength(50).WithMessage("Key must be 50 characters or fewer.");
        
        RuleFor(x => x.Translations)
            .NotEmpty().WithMessage("At least one translation is required.");
        RuleForEach(x => x.Translations)
            .SetValidator(new SkillTranslationDtoValidator());
    }
}