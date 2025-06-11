namespace PersonalSite.Domain.Validation.Translations;

public class PageTranslationValidator : AbstractValidator<PageTranslation>
{
    public PageTranslationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Translation ID is required.");
        
        RuleFor(x => x.PageId)
            .NotEmpty().WithMessage("PageId is required.");

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