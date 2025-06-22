using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Domain.Entities.Skills;

[Table("SkillCategories")]
public class SkillCategory
{
    [Key]
    public Guid Id { get; set; }
    [Required, MaxLength(50)]
    public string Key { get; set; } = string.Empty;
    public short DisplayOrder { get; set; }
    public ICollection<SkillCategoryTranslation> Translations { get; set; } = [];
}