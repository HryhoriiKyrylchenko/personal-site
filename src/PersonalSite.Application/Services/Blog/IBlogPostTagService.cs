using PersonalSite.Application.Features.Blog.Common.Dtos;

namespace PersonalSite.Application.Services.Blog;

public interface IBlogPostTagService : ICrudService<BlogPostTagDto, BlogPostTagAddRequest, BlogPostTagUpdateRequest>
{
}