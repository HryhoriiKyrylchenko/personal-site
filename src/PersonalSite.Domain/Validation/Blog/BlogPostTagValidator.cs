namespace PersonalSite.Domain.Validation.Blog;

public class BlogPostTagValidator : AbstractValidator<BlogPostTag>
{
    public BlogPostTagValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Tag ID is required.");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tag name is required.")
            .MaximumLength(50).WithMessage("Tag name must be 50 characters or fewer.");
    }
}