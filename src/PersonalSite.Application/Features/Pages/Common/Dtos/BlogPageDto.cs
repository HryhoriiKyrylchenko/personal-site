namespace PersonalSite.Application.Features.Pages.Common.Dtos;

public class BlogPageDto
{
    public PageDto? PageData { get; set; } = null!;
    public IReadOnlyList<BlogPostDto> BlogPosts { get; set; } = null!;
}