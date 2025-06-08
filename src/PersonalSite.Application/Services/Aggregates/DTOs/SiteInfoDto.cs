namespace PersonalSite.Application.Services.Aggregates.DTOs;

public class SiteInfoDto
{
    public IReadOnlyList<LanguageDto> Languages { get; set; } = [];
    public IReadOnlyList<SocialMediaLinkDto> SocialLinks { get; set; } = [];
    public ResumeDto? Resume { get; set; }
}