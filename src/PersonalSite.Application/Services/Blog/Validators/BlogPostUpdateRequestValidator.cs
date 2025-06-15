namespace PersonalSite.Application.Services.Blog.Validators;

public class BlogPostUpdateRequestValidator : AbstractValidator<BlogPostUpdateRequest>
{
    public BlogPostUpdateRequestValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Post ID is required.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required.")
            .MaximumLength(100).WithMessage("Slug must be 100 characters or fewer.");

        RuleFor(x => x.CoverImage)
            .MaximumLength(255).WithMessage("Cover image URL must be 255 characters or fewer.");
    }
}