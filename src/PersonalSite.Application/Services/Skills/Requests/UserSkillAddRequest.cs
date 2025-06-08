namespace PersonalSite.Application.Services.Skills.Requests;

public class UserSkillAddRequest
{
    public Guid SkillId { get; set; }
    public short Proficiency { get; set; }
}