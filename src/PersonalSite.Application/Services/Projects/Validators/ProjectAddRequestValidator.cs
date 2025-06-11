namespace PersonalSite.Application.Services.Projects.Validators;

public class ProjectAddRequestValidator : AbstractValidator<ProjectAddRequest>
{
    public ProjectAddRequestValidator()
    {
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
    }

    private bool BeValidUrlOrEmpty(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return true;

        return Uri.TryCreate(url, UriKind.Absolute, out _);
    }
}