namespace PersonalSite.Application.Services.Blog.Validators;

public class BlogPostAddRequestValidator : AbstractValidator<BlogPostAddRequest>
{
    public BlogPostAddRequestValidator()
    {
        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required.")
            .MaximumLength(100).WithMessage("Slug must be 100 characters or fewer.");

        RuleFor(x => x.CoverImage)
            .MaximumLength(255).WithMessage("Cover image URL must be 255 characters or fewer.");
    }
}