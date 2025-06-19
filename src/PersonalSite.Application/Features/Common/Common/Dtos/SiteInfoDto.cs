namespace PersonalSite.Application.Features.Common.Common.Dtos;

public class SiteInfoDto
{
    public IReadOnlyList<LanguageDto> Languages { get; set; } = [];
    public IReadOnlyList<SocialMediaLinkDto> SocialLinks { get; set; } = [];
    public ResumeDto? Resume { get; set; }
}