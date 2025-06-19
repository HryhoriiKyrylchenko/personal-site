namespace PersonalSite.Application.Features.Blog.Commands.CreateBlogPost;

public class BlogPostTranslationDtoValidator : AbstractValidator<BlogPostTranslationDto>
{
    public BlogPostTranslationDtoValidator()
    {
        RuleFor(x => x.LanguageCode)
            .NotEmpty().WithMessage("Language code is required.")
            .Length(2, 2).WithMessage("Language code must be 2 characters.");

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
            .MaximumLength(255).WithMessage("OG image path must be 255 characters or fewer.");
    }
}