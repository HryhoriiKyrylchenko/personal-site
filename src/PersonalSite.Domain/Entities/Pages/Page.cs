namespace PersonalSite.Domain.Entities.Pages;

[Table("Pages")]
public class Page
{
    [Key] public Guid Id { get; set; }
    [Required, MaxLength(50)] public string Key { get; set; } = string.Empty;
    public virtual ICollection<PageTranslation> Translations { get; set; } = [];
}