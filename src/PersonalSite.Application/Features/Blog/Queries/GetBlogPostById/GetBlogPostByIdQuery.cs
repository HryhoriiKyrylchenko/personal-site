namespace PersonalSite.Application.Features.Blog.Queries.GetBlogPostById;

public record GetBlogPostByIdQuery(Guid Id) : IRequest<Result<BlogPostAdminDto>>;