namespace PersonalSite.Application.Features.Pages.Page.Dtos;

public class PageDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string PageImage { get; set; } = string.Empty;
    public string LanguageCode { get; set; } = string.Empty;
    public Dictionary<string, string> Data { get; set; } = [];
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    
    public string MetaTitle { get; set; } = string.Empty;
    public string MetaDescription { get; set; } = string.Empty;
    public string OgImage { get; set; } = string.Empty;
}