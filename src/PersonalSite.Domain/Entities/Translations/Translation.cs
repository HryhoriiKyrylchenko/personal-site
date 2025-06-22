using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Domain.Entities.Translations;

[Table("Translations")]
public abstract class Translation
{
    [Key]
    public Guid Id { get; set; }
    [Required, MaxLength(2)]
    public Guid LanguageId { get; set; }
    [ForeignKey("LanguageId")]
    public virtual Language Language { get; set; } = null!;
}