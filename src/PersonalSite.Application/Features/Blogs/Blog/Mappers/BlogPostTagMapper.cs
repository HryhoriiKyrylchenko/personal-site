namespace PersonalSite.Application.Features.Blogs.Blog.Mappers;

public static class BlogPostTagMapper
{
    public static BlogPostTagDto MapToDto(BlogPostTag entity)
    {
        return new BlogPostTagDto
        {
            Id = entity.Id,
            Name = entity.Name
        };
    }
    
    public static List<BlogPostTagDto> MapToDtoList(IEnumerable<BlogPostTag> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}