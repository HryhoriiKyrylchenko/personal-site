using PersonalSite.Domain.Entities.Blog;

namespace PersonalSite.Domain.Validation.Blog;

public class PostTagValidator : AbstractValidator<PostTag>
{
    public PostTagValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("PostTag ID is required.");

        RuleFor(x => x.BlogPostId)
            .NotEmpty().WithMessage("BlogPostId is required.");

        RuleFor(x => x.BlogPostTagId)
            .NotEmpty().WithMessage("BlogPostTagId is required.");
    }
}