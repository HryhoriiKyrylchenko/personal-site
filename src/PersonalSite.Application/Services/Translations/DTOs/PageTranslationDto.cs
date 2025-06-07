namespace PersonalSite.Application.Services.Translations.DTOs;

public class PageTranslationDto
{
    public Guid PageId { get; set; }
    public Dictionary<string, string> Data { get; set; } = [];
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
    public string OgImage { get; set; } = string.Empty;
}