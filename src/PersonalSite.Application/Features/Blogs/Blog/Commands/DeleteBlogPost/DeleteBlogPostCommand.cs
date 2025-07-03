using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Blogs.Blog.Commands.DeleteBlogPost;

public record DeleteBlogPostCommand(Guid Id) : IRequest<Result>;