namespace PersonalSite.Application.Services.Translations.DTOs;

public class LanguageDto
{
    public Guid Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
}