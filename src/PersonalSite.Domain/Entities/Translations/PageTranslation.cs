using PersonalSite.Domain.Entities.Pages;

namespace PersonalSite.Domain.Entities.Translations;

[Table("PageTranslations")]
public class PageTranslation : Translation
{
    [Required]
    public Guid PageId { get; set; }
    public virtual Page Page { get; set; } = null!;
    [Column(TypeName = "jsonb")]
    public Dictionary<string, string> Data { get; set; } = [];
    [Required, MaxLength(255)]
    public string Title { get; set; } = string.Empty;
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;
    
    [MaxLength(255)]
    public string MetaTitle { get; set; } = string.Empty;
    [MaxLength(500)]
    public string MetaDescription { get; set; } = string.Empty;
    [MaxLength(255)]
    public string OgImage { get; set; } = string.Empty;
}