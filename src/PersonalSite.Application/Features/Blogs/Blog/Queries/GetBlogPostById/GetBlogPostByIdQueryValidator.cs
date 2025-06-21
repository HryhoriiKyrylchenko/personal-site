namespace PersonalSite.Application.Features.Blogs.Blog.Queries.GetBlogPostById;

public class GetBlogPostByIdQueryValidator : AbstractValidator<GetBlogPostByIdQuery>
{
    public GetBlogPostByIdQueryValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Blog post ID is required.");
    }
}