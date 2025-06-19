namespace PersonalSite.Application.Services.Skills.Requests;

public class ProjectSkillAddRequest
{
    public Guid ProjectId { get; set; }
    public Guid SkillId { get; set; }
}