namespace PersonalSite.Application.Services.Skills.DTOs;

public class SkillCategoryDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public short DisplayOrder { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}