namespace PersonalSite.Application.Services.Translations.Requests;

public class ProjectTranslationUpdateRequest
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string ShortDescription { get; set; } = string.Empty;
    public Dictionary<string, string> DescriptionSections { get; set; } = [];
    
    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
    public string OgImage { get; set; } = string.Empty;
}