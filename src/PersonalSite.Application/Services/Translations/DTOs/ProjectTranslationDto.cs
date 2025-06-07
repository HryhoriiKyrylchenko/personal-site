namespace PersonalSite.Application.Services.Translations.DTOs;

public class ProjectTranslationDto
{
    public Guid Id { get; set; }
    public Guid LanguageId { get; set; }
    public Guid ProjectId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public Dictionary<string, string> DescriptionSections { get; set; } = [];

    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
    public string OgImage { get; set; } = string.Empty;
}