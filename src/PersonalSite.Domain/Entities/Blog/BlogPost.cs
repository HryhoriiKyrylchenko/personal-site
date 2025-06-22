using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Domain.Entities.Blog;

[Table("BlogPosts")]
public class BlogPost : SoftDeletableEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required, MaxLength(100)]
    public string Slug { get; set; } = string.Empty;
    [MaxLength(255)]
    public string CoverImage { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    
    public bool IsPublished { get; set; }
    public DateTime? PublishedAt { get; set; }
    
    public virtual ICollection<BlogPostTranslation> Translations { get; set; } = [];
    public virtual ICollection<PostTag> PostTags { get; set; } = [];
}