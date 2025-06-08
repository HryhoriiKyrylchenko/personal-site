namespace PersonalSite.Domain.Entities.Common;

[Table("SocialMediaLinks")]
public class SocialMediaLink
{
    [Key]
    public Guid Id { get; set; }

    [Required, MaxLength(50)]
    public string Platform { get; set; } = string.Empty;

    [Required, MaxLength(255)]
    public string Url { get; set; } = string.Empty;

    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
