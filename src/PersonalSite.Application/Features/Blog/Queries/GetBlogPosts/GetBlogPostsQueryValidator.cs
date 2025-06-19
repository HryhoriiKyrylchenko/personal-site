namespace PersonalSite.Application.Features.Blog.Queries.GetBlogPosts;

public class GetBlogPostsQueryValidator : AbstractValidator<GetBlogPostsQuery>
{
    public GetBlogPostsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page number must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.SlugFilter)
            .MaximumLength(100).WithMessage("Slug filter must be 100 characters or fewer.");
    }
}