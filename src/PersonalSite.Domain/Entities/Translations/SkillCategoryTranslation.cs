namespace PersonalSite.Domain.Entities.Translations;

[Table("SkillCategoryTranslations")]
public class SkillCategoryTranslation : Translation
{
    [Required]
    public Guid SkillCategoryId { get; set; }
    [ForeignKey("SkillCategoryId")]
    public virtual SkillCategory SkillCategory { get; set; } = null!;

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;
}