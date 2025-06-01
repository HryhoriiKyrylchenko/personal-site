namespace PersonalSite.Domain.Entities.Translations;

[Table("Translations")]
public abstract class Translation
{
    [Key]
    public Guid Id { get; set; }
    [Required, MaxLength(2)]
    public string LanguageCode { get; set; } = string.Empty;
    [ForeignKey("LanguageCode")]
    public Language Language { get; set; } = null!;
}