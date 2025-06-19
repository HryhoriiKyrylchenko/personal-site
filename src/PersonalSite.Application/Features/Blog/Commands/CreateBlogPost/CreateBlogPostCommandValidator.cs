namespace PersonalSite.Application.Features.Blog.Commands.CreateBlogPost;

public class CreateBlogPostCommandValidator : AbstractValidator<CreateBlogPostCommand>
{
    public CreateBlogPostCommandValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required.")
            .MaximumLength(100).WithMessage("Slug must be 100 characters or fewer.");

        RuleFor(x => x.CoverImage)
            .NotEmpty().WithMessage("Cover image is required.")
            .MaximumLength(255).WithMessage("Cover image path must be 255 characters or fewer.");

        RuleFor(x => x.Translations)
            .NotEmpty().WithMessage("At least one translation is required.");

        RuleForEach(x => x.Translations)
            .SetValidator(new BlogPostTranslationDtoValidator());

        RuleForEach(x => x.Tags)
            .SetValidator(new BlogPostTagDtoValidator());
    }
}