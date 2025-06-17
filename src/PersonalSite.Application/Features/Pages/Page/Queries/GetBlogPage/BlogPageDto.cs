using PersonalSite.Application.Features.Blog.Common.Dtos;
using PersonalSite.Application.Features.Pages.Common.Dtos;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetBlogPage;

public class BlogPageDto
{
    public PageDto? PageData { get; set; } = null!;
    public IReadOnlyList<BlogPostDto> BlogPosts { get; set; } = null!;
}