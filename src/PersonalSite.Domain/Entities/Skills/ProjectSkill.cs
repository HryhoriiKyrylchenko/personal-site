namespace PersonalSite.Domain.Entities.Skills;

[Table("ProjectSkills")]
public class ProjectSkill
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid ProjectId { get; set; }
    [ForeignKey("ProjectId")]
    public virtual Project Project { get; set; } = null!;
    [Required]
    public Guid SkillId { get; set; }
    [ForeignKey("SkillId")]
    public virtual Skill Skill { get; set; } = null!;
}