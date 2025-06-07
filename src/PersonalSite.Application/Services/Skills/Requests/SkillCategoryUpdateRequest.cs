namespace PersonalSite.Application.Services.Skills.Requests;

public class SkillCategoryUpdateRequest
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public short DisplayOrder { get; set; }
}