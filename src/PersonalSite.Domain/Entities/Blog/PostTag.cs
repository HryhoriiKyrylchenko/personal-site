namespace PersonalSite.Domain.Entities.Blog;

[Table("PostTags")]
public class PostTag
{
    [Key]
    public Guid Id { get; set; }
    
    [Required]
    public Guid BlogPostId { get; set; }
    public virtual BlogPost BlogPost { get; set; } = null!;
    
    [Required]
    public Guid BlogPostTagId { get; set; }
    public virtual BlogPostTag BlogPostTag { get; set; } = null!;
}