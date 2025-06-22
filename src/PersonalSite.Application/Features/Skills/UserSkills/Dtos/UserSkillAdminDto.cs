using PersonalSite.Application.Features.Skills.Skills.Dtos;

namespace PersonalSite.Application.Features.Skills.UserSkills.Dtos;

public class UserSkillAdminDto
{
    public Guid Id { get; set; }
    public SkillAdminDto Skill { get; set; } = null!;
    public short Proficiency { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}