namespace PersonalSite.Application.Services.Skills.Validators;

public class ProjectSkillAddRequestValidator : AbstractValidator<ProjectSkillAddRequest>
{
    public ProjectSkillAddRequestValidator()
    {
        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("ProjectId is required.");

        RuleFor(x => x.SkillId)
            .NotEmpty().WithMessage("SkillId is required.");
    }
}