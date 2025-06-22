using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;

namespace PersonalSite.Application.Features.Skills.SkillCategories.Commands.CreateSkillCategory;

public class SkillCategoryTranslationDtoValidator : AbstractValidator<SkillCategoryTranslationDto>
{
    public SkillCategoryTranslationDtoValidator()
    {
        RuleFor(x => x.LanguageCode)
            .NotEmpty().WithMessage("Language code is required.")
            .Length(2).WithMessage("Language code must be exactly 2 characters.");

        RuleFor(x => x.SkillCategoryId)
            .NotEmpty().WithMessage("SkillCategoryId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must be 100 characters or fewer.");

        RuleFor(x => x.Description)
            .MaximumLength(255).WithMessage("Description must be 255 characters or fewer.");
    }
}