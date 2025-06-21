namespace PersonalSite.Domain.Entities.Skills;

[Table("Skills")]
public class Skill : SoftDeletableEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid CategoryId { get; set; }
    [ForeignKey("CategoryId")]
    public virtual SkillCategory Category { get; set; } = null!;
    [Required, MaxLength(50)]
    public string Key { get; set; } = string.Empty;

    public virtual ICollection<ProjectSkill> ProjectSkills { get; set; } = [];
    public virtual ICollection<UserSkill> UserSkills { get; set; } = [];
    public virtual ICollection<LearningSkill> LearningSkills { get; set; } = [];
    public virtual ICollection<SkillTranslation> Translations { get; set; } = [];
}