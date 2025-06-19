namespace PersonalSite.Application.Features.Blog.Queries.GetBlogPosts;

public record GetBlogPostsQuery(
    int Page = 1,
    int PageSize = 10,
    string? SlugFilter = null,
    bool? IsPublishedFilter = null
) : IRequest<PaginatedResult<BlogPostAdminDto>>;