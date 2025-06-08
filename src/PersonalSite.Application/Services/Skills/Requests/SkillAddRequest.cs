namespace PersonalSite.Application.Services.Skills.Requests;

public class SkillAddRequest
{
    public Guid CategoryId { get; set; }
    public string Key { get; set; } = string.Empty;
}