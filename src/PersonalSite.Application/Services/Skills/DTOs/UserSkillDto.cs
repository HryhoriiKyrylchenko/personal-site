namespace PersonalSite.Application.Services.Skills.DTOs;

public class UserSkillDto
{
    public Guid Id { get; set; }
    public SkillDto Skill { get; set; } = null!;
    public short Proficiency { get; set; }
}