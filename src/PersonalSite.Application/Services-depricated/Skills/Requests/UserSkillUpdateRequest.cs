namespace PersonalSite.Application.Services.Skills.Requests;

public class UserSkillUpdateRequest
{
    public Guid Id { get; set; }
    public short Proficiency { get; set; }
}