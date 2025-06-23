using PersonalSite.Application.Features.Projects.Project.Commands.CreateProject;

namespace PersonalSite.Application.Features.Projects.Project.Commands.UpdateProject;

public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
{
    public UpdateProjectCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Project ID is required.");
        
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required.")
            .MaximumLength(100).WithMessage("Slug must be 100 characters or fewer.");

        RuleFor(x => x.CoverImage)
            .MaximumLength(255).WithMessage("CoverImage must be 255 characters or fewer.");
        
        RuleFor(x => x.DemoUrl)
            .MaximumLength(255).WithMessage("DemoUrl must be 255 characters or fewer.")
            .Must(BeValidUrlOrEmpty).WithMessage("DemoUrl must be a valid URL or empty.");

        RuleFor(x => x.RepoUrl)
            .MaximumLength(255).WithMessage("RepoUrl must be 255 characters or fewer.")
            .Must(BeValidUrlOrEmpty).WithMessage("RepoUrl must be a valid URL or empty.");
        
        RuleFor(x => x.Translations)
            .NotEmpty().WithMessage("At least one translation is required.");
        
        RuleForEach(x => x.Translations).SetValidator(new ProjectTranslationDtoValidator());
        
        RuleForEach(x => x.SkillIds).NotEmpty().WithMessage("At least one skill is required.");
    }
    
    private bool BeValidUrlOrEmpty(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return true;

        return Uri.TryCreate(url, UriKind.Absolute, out var result)
               && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }
}