namespace PersonalSite.Application.Features.Skills.SkillCategories.Commands.CreateSkillCategory;

public class CreateSkillCategoryCommandValidator : AbstractValidator<CreateSkillCategoryCommand>
{
    public CreateSkillCategoryCommandValidator()
    {
        RuleFor(x => x.Key)
            .NotEmpty().WithMessage("Key is required.")
            .MaximumLength(50).WithMessage("Key must be 50 characters or fewer.");
        RuleFor(x => x.DisplayOrder)
            .GreaterThanOrEqualTo((short)0).WithMessage("DisplayOrder must be non-negative.");
        RuleFor(x => x.Translations)
            .NotEmpty().WithMessage("At least one translation is required.");
        RuleForEach(x => x.Translations)
            .SetValidator(new SkillCategoryTranslationDtoValidator());
    }
}