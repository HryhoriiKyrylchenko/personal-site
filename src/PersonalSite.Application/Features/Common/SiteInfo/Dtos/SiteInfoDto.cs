using PersonalSite.Application.Features.Common.Language.Dtos;
using PersonalSite.Application.Features.Common.Resume.Dtos;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;

namespace PersonalSite.Application.Features.Common.SiteInfo.Dtos;

public class SiteInfoDto
{
    public IReadOnlyList<LanguageDto> Languages { get; set; } = [];
    public IReadOnlyList<SocialMediaLinkDto> SocialLinks { get; set; } = [];
    public ResumeDto? Resume { get; set; }
}