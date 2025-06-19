namespace PersonalSite.Application.Features.Blog.Common.Dtos;

public class BlogPostAdminDto
{
    public Guid Id { get; set; }
    public string Slug { get; set; } = string.Empty;
    public string CoverImage { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    
    public List<BlogPostTranslationDto> Translations { get; set; } = [];
    public List<BlogPostTagDto> Tags { get; set; } = [];
}