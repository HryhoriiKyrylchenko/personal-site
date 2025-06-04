namespace PersonalSite.Domain.Entities.Translations;

[Table("SkillTranslations")]
public class SkillTranslation : Translation
{
    [Required]
    public Guid SkillId { get; set; }
    [ForeignKey("SkillId")] 
    public virtual Skill Skill { get; set; } = null!;
    
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}