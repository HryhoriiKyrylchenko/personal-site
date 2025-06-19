namespace PersonalSite.Application.Services.Translations.Requests;

public class SkillCategoryTranslationAddRequest
{
    public Guid LanguageId { get; set; }
    public Guid SkillCategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}