namespace PersonalSite.Application.Services.Skills.DTOs;

public class ProjectSkillDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public SkillDto Skill { get; set; } = null!;
}