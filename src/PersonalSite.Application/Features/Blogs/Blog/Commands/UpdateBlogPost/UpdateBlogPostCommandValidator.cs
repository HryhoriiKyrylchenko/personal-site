namespace PersonalSite.Application.Features.Blogs.Blog.Commands.UpdateBlogPost;

public class UpdateBlogPostCommandValidator : AbstractValidator<UpdateBlogPostCommand>
{
    public UpdateBlogPostCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Blog post ID is required.");

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
