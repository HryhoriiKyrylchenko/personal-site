namespace PersonalSite.Application.Features.Skills.Common.Dtos;

public class ProjectSkillDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public SkillDto Skill { get; set; } = null!;
}