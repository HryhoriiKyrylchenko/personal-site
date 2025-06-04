namespace PersonalSite.Domain.Entities.Skills;

[Table("UserSkills")]
public class UserSkill : SoftDeletableEntity
{
    [Key]
    public Guid Id { get; set; }
    [Required]
    public Guid SkillId { get; set; }
    [ForeignKey("SkillId")]
    public virtual Skill Skill { get; set; } = null!;
    [Range(1,5)]
    public short Proficiency { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}