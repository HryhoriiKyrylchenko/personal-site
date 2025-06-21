namespace PersonalSite.Application.Features.Blogs.Blog.Commands.PublishBlogPost;

public class PublishBlogPostCommandValidator : AbstractValidator<PublishBlogPostCommand>
{
    public PublishBlogPostCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Blog post ID is required.");

        When(x => x.IsPublished, () =>
        {
            RuleFor(x => x.PublishDate)
                .NotNull().WithMessage("Publish date is required when publishing.");
        });
    }
}