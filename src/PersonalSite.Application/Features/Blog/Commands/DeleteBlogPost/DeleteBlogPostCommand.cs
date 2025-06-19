namespace PersonalSite.Application.Features.Blog.Commands.DeleteBlogPost;

public record DeleteBlogPostCommand(Guid Id) : IRequest<Result>;