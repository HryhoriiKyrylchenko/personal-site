namespace PersonalSite.Application.Features.Common.SiteInfo.Dtos;

public class SiteInfoDto
{
    public IReadOnlyList<LanguageDto> Languages { get; set; } = [];
    public IReadOnlyList<SocialMediaLinkDto> SocialLinks { get; set; } = [];
    public ResumeDto? Resume { get; set; }
}