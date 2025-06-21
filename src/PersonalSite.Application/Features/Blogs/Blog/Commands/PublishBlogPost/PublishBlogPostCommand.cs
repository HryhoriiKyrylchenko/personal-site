namespace PersonalSite.Application.Features.Blogs.Blog.Commands.PublishBlogPost;

public class PublishBlogPostCommand : IRequest<Result>
{
    public Guid Id { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishDate { get; set; }
}