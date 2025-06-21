namespace PersonalSite.Application.Features.Skills.Skills.Commands.CreateSkill;

public class SkillTranslationDtoValidator : AbstractValidator<SkillTranslationDto>
{
    public SkillTranslationDtoValidator()
    {
        RuleFor(x => x.LanguageCode)
            .NotEmpty().WithMessage("Language code is required.")
            .Length(2).WithMessage("Language code must be exactly 2 characters.");

        RuleFor(x => x.SkillId)
            .NotEmpty().WithMessage("SkillId is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .MaximumLength(100).WithMessage("Name must be 100 characters or fewer.");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must be 1000 characters or fewer.");
    }
}