namespace PersonalSite.Application.Features.Projects.Project.Dtos;

public class ProjectSkillAdminDto
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public SkillAdminDto Skill { get; set; } = null!;
}