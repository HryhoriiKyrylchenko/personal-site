namespace PersonalSite.Application.Services.Translations.DTOs;

public class SkillCategoryTranslationDto
{
    public Guid Id { get; set; }
    public Guid LanguageId { get; set; }
    public Guid SkillCategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}