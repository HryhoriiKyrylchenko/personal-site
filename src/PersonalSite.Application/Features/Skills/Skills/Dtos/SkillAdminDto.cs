using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;

namespace PersonalSite.Application.Features.Skills.Skills.Dtos;

public class SkillAdminDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public SkillCategoryAdminDto Category { get; set; } = null!;
    public List<SkillTranslationDto> Translations { get; set; } = null!;
}