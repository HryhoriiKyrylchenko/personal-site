namespace PersonalSite.Application.Services.Translations.Requests;

public class SkillTranslationUpdateRequest
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}