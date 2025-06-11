namespace PersonalSite.Application.Services.Blog.Validators;

public class BlogPostTagAddRequestValidator : AbstractValidator<BlogPostTagAddRequest>
{
    public BlogPostTagAddRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tag name is required.")
            .MaximumLength(50).WithMessage("Tag name must be 50 characters or fewer.");
    }
}