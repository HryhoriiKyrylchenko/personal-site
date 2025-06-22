using PersonalSite.Domain.Entities.Blog;

namespace PersonalSite.Domain.Entities.Translations;

[Table("BlogPostTranslations")]
public class BlogPostTranslation : Translation
{
    [Required]
    public Guid BlogPostId { get; set; }
    public virtual BlogPost BlogPost { get; set; } = null!;
    
    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

    [MaxLength(255)]
    public string MetaTitle { get; set; } = string.Empty;
    [MaxLength(500)]
    public string MetaDescription { get; set; } = string.Empty;
    [MaxLength(255)]
    public string OgImage { get; set; } = string.Empty;
}