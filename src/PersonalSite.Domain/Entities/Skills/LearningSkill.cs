namespace PersonalSite.Domain.Entities.Skills;

[Table("LearningSkills")]
public class LearningSkill : SoftDeletableEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid SkillId { get; set; }
    [ForeignKey("SkillId")]
    public virtual Skill Skill { get; set; } = null!;
    [Required]
    public LearningStatus LearningStatus { get; set; }
    public short DisplayOrder { get; set; }
}