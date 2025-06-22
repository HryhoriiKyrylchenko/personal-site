using PersonalSite.Application.Features.Skills.Skills.Dtos;

namespace PersonalSite.Application.Features.Skills.UserSkills.Dtos;

public class UserSkillDto
{
    public Guid Id { get; set; }
    public SkillDto Skill { get; set; } = null!;
    public short Proficiency { get; set; }
}