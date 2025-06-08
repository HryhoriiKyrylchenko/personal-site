namespace PersonalSite.Application.Services.Aggregates.DTOs;

public class BlogPageDto
{
    public PageDto? PageData { get; set; } = null!;
    public IReadOnlyList<BlogPostDto> BlogPosts { get; set; } = null!;
}