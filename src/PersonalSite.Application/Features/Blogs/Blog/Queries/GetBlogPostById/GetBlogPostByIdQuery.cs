using PersonalSite.Application.Features.Blogs.Blog.Dtos;

namespace PersonalSite.Application.Features.Blogs.Blog.Queries.GetBlogPostById;

public record GetBlogPostByIdQuery(Guid Id) : IRequest<Result<BlogPostAdminDto>>;