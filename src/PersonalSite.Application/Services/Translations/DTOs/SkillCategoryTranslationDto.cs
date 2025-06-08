namespace PersonalSite.Application.Services.Translations.DTOs;

public class SkillCategoryTranslationDto
{
    public Guid Id { get; set; }
    public string LanguageCode { get; set; } = string.Empty;
    public Guid SkillCategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}