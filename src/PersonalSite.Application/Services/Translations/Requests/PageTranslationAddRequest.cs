namespace PersonalSite.Application.Services.Translations.Requests;

public class PageTranslationAddRequest
{
    public Guid LanguageId { get; set; }
    public Guid PageId { get; set; }
    public Dictionary<string, string> Data { get; set; } = [];
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
    public string OgImage { get; set; } = string.Empty;
}