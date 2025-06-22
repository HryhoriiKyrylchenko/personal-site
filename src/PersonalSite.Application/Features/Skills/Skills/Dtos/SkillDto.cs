using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;

namespace PersonalSite.Application.Features.Skills.Skills.Dtos;

public class SkillDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public SkillCategoryDto Category { get; set; } = null!;
}