namespace PersonalSite.Application.Services.Translations.Requests;

public class BlogPostTranslationAddRequest
{
    public Guid LanguageId { get; set; }
    public Guid BlogPostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    
    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
    public string OgImage { get; set; } = string.Empty;
}