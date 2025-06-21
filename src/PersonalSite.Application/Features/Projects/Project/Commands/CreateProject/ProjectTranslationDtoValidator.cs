using PersonalSite.Application.Features.Projects.Project.Dtos;

namespace PersonalSite.Application.Features.Projects.Project.Commands.CreateProject;

public class ProjectTranslationDtoValidator : AbstractValidator<ProjectTranslationDto>
{
    public ProjectTranslationDtoValidator()
    {
        RuleFor(x => x.LanguageCode).NotEmpty().WithMessage("Language code is required")
            .MaximumLength(10).WithMessage("Language code must be 10 characters or fewer.");
            
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