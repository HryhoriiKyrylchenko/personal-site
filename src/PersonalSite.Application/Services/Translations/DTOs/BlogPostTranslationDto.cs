namespace PersonalSite.Application.Services.Translations.DTOs;

public class BlogPostTranslationDto
{
    public Guid Id { get; set; }
    public string LanguageCode { get; set; } = string.Empty;
    public Guid BlogPostId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Excerpt { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
    public string OgImage { get; set; } = string.Empty;
}