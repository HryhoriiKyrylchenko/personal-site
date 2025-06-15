namespace PersonalSite.Domain.Validation.Skills;

public class ProjectSkillValidator : AbstractValidator<ProjectSkill>
{
    public ProjectSkillValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("ProjectSkill ID is required.");

        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("ProjectId is required.");

        RuleFor(x => x.SkillId)
            .NotEmpty().WithMessage("SkillId is required.");
    }
}