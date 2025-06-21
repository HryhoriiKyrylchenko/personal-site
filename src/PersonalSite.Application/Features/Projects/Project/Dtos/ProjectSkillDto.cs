using PersonalSite.Application.Features.Skills.Skills.Dtos;

namespace PersonalSite.Application.Features.Projects.Project.Dtos;

public class ProjectSkillDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public SkillDto Skill { get; set; } = null!;
}