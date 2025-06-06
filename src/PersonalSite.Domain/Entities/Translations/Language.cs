namespace PersonalSite.Domain.Entities.Translations;

[Table("Languages")]
public class Language
{
    [Key]
    public Guid Id { get; set; }
    [MaxLength(2)]
    public string Code { get; set; } = string.Empty;
    [Required]
    public string Name { get; set; } = string.Empty;
}