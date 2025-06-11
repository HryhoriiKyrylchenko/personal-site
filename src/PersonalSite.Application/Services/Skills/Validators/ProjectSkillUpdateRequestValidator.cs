namespace PersonalSite.Application.Services.Skills.Validators;

public class ProjectSkillUpdateRequestValidator : AbstractValidator<ProjectSkillUpdateRequest>
{
    public ProjectSkillUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ProjectSkill ID is required.");

        RuleFor(x => x.SkillId)
            .NotEmpty().WithMessage("SkillId is required.");
    }
}