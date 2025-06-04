namespace PersonalSite.Domain.Entities.Translations;

[Table("ProjectTranslations")]
public class ProjectTranslation : Translation
{
    [Required]
    public Guid ProjectId { get; set; }
    [ForeignKey("ProjectId")] 
    public virtual Project Project { get; set; } = null!;

    [Required, MaxLength(200)]
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    [Column(TypeName = "jsonb")]
    public Dictionary<string, string> DescriptionSections { get; set; } = [];

    [MaxLength(255)]
    public string MetaTitle { get; set; } = string.Empty;
    [MaxLength(500)]
    public string MetaDescription { get; set; } = string.Empty;
    [MaxLength(255)]
    public string OgImage { get; set; } = string.Empty;
}