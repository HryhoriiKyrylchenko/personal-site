namespace PersonalSite.Application.Features.Skills.Skills.Dtos;

public class SkillTranslationDto
{
    public Guid Id { get; set; }
    public string LanguageCode { get; set; } = string.Empty;
    public Guid SkillId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}