using PersonalSite.Application.Features.Blogs.Blog.Dtos;
using PersonalSite.Domain.Entities.Blog;

namespace PersonalSite.Application.Features.Blogs.Blog.Mappers;

public class BlogPostTagMapper : IMapper<BlogPostTag, BlogPostTagDto>
{
    public BlogPostTagDto MapToDto(BlogPostTag entity)
    {
        return new BlogPostTagDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
    
    public List<BlogPostTagDto> MapToDtoList(IEnumerable<BlogPostTag> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}