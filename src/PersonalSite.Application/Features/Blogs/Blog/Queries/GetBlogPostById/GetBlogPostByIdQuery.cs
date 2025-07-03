using PersonalSite.Application.Features.Blogs.Blog.Dtos;
using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Blogs.Blog.Queries.GetBlogPostById;

public record GetBlogPostByIdQuery(Guid Id) : IRequest<Result<BlogPostAdminDto>>;