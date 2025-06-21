namespace PersonalSite.Application.Features.Pages.Page.Commands.CreatePage;

public class PageTranslationDtoValidator : AbstractValidator<PageTranslationDto>
{
    public PageTranslationDtoValidator()
    {
        RuleFor(x => x.LanguageCode).NotEmpty().WithMessage("LanguageCode is required.")
            .MaximumLength(10).WithMessage("LanguageCode must be 10 characters or fewer.");
        
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(255).WithMessage("Title must be 255 characters or fewer.");

        RuleFor(x => x.Description)
            .MaximumLength(500).WithMessage("Description must be 500 characters or fewer.");
        RuleFor(x => x.MetaTitle)
            .MaximumLength(255).WithMessage("MetaTitle must be 255 characters or fewer.");

        RuleFor(x => x.MetaDescription)
            .MaximumLength(500).WithMessage("MetaDescription must be 500 characters or fewer.");

        RuleFor(x => x.OgImage)
            .MaximumLength(255).WithMessage("OgImage must be 255 characters or fewer.");
    }
}