using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Domain.Validation.Translations;

public class ProjectTranslationValidator : AbstractValidator<ProjectTranslation>
{
    public ProjectTranslationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Translation ID is required.");
        
        RuleFor(x => x.ProjectId)
            .NotEmpty().WithMessage("ProjectId is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must be 200 characters or fewer.");

        RuleFor(x => x.MetaTitle)
            .MaximumLength(255).WithMessage("MetaTitle must be 255 characters or fewer.");

        RuleFor(x => x.MetaDescription)
            .MaximumLength(500).WithMessage("MetaDescription must be 500 characters or fewer.");

        RuleFor(x => x.OgImage)
            .MaximumLength(255).WithMessage("OgImage must be 255 characters or fewer.");
    }
}