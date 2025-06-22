using PersonalSite.Application.Features.Blogs.Blog.Dtos;

namespace PersonalSite.Application.Features.Pages.Page.Dtos;

public class BlogPageDto
{
    public PageDto? PageData { get; set; } = null!;
    public IReadOnlyList<BlogPostDto> BlogPosts { get; set; } = null!;
}