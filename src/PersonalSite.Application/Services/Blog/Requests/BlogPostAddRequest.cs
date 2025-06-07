namespace PersonalSite.Application.Services.Blog.Requests;

public class BlogPostAddRequest
{
    public string Slug { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
}