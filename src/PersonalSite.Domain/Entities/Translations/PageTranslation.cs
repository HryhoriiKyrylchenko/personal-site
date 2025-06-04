namespace PersonalSite.Domain.Entities.Translations;

[Table("PageTranslations")]
public class PageTranslation : Translation
{
    [Required, MaxLength(50)]
    public string PageKey { get; set; } = string.Empty;
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