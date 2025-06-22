namespace PersonalSite.Domain.Entities.Blog;

[Table("BlogPostTags")]
public class BlogPostTag
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    public virtual ICollection<PostTag> PostTags { get; set; } = [];
}