namespace PersonalSite.Application.Services.Translations.DTOs;

public class SkillTranslationDto
{
    public Guid Id { get; set; }
    public Guid LanguageId { get; set; }
    public Guid SkillId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}