using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Domain.Validation.Translations;

public class BlogPostTranslationValidator : AbstractValidator<BlogPostTranslation>
{
    public BlogPostTranslationValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Translation ID is required.");
        
        RuleFor(x => x.BlogPostId)
            .NotEmpty().WithMessage("BlogPostId is required.");

        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(200).WithMessage("Title must be 200 characters or fewer.");

        RuleFor(x => x.Excerpt)
            .NotNull();

        RuleFor(x => x.Content)
            .NotNull(); 

        RuleFor(x => x.MetaTitle)
            .MaximumLength(255).WithMessage("MetaTitle must be 255 characters or fewer.");

        RuleFor(x => x.MetaDescription)
            .MaximumLength(500).WithMessage("MetaDescription must be 500 characters or fewer.");

        RuleFor(x => x.OgImage)
            .MaximumLength(255).WithMessage("OgImage must be 255 characters or fewer.");
    }
}