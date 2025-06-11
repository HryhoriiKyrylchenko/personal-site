namespace PersonalSite.Domain.Validation.Blog;

public class BlogPostValidator : AbstractValidator<BlogPost>
{
    public BlogPostValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Post ID is required.");

        RuleFor(x => x.Slug)
            .NotEmpty().WithMessage("Slug is required.")
            .MaximumLength(100).WithMessage("Slug must be 100 characters or fewer.");

        RuleFor(x => x.CoverImage)
            .MaximumLength(255).WithMessage("Cover image URL must be 255 characters or fewer.");

        RuleFor(x => x.CreatedAt)
            .NotEmpty().WithMessage("CreatedAt is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("CreatedAt cannot be in the future.");

        RuleFor(x => x.UpdatedAt)
            .Must(updatedAt => !updatedAt.HasValue || updatedAt <= DateTime.UtcNow)
            .WithMessage("UpdatedAt cannot be in the future.")
            .Must((post, updatedAt) =>
                !updatedAt.HasValue || updatedAt >= post.CreatedAt)
            .WithMessage("UpdatedAt cannot be before CreatedAt.");

        RuleFor(x => x.PublishedAt)
            .LessThanOrEqualTo(DateTime.UtcNow)
            .When(x => x.PublishedAt.HasValue)
            .WithMessage("PublishedAt cannot be in the future.");

        RuleFor(x => x)
            .Must(post => !post.IsPublished || post.PublishedAt.HasValue)
            .WithMessage("PublishedAt must be set when IsPublished is true.");
    }
}