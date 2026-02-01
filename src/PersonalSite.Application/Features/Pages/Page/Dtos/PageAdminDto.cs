namespace PersonalSite.Application.Features.Pages.Page.Dtos;

public class PageAdminDto
{
    public Guid Id { get; set; }
    public string Key { get; set; } = string.Empty;
    public string PageImage { get; set; } = string.Empty;
    public List<PageTranslationDto> Translations { get; set; } = [];
}