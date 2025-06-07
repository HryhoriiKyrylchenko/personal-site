namespace PersonalSite.Application.Services.Blog.Requests;

public class BlogPostUpdateRequest
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
}