namespace PersonalSite.Application.Services.Skills.Requests;

public class SkillUpdateRequest
{
    public Guid Id { get; set; }
    public Guid CategoryId { get; set; }
    public string Key { get; set; } = string.Empty;
}