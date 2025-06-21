namespace PersonalSite.Application.Features.Skills.SkillCategories.Dtos;

public class SkillCategoryAdminDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public short DisplayOrder { get; set; }
    public List<SkillCategoryTranslationDto> Translations { get; set; } = null!;   
}