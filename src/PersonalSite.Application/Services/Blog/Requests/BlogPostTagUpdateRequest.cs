namespace PersonalSite.Application.Services.Blog.Requests;

public class BlogPostTagUpdateRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}