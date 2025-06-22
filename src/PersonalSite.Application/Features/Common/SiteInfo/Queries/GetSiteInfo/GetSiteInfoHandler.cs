using PersonalSite.Application.Features.Common.Language.Dtos;
using PersonalSite.Application.Features.Common.Resume.Dtos;
using PersonalSite.Application.Features.Common.SiteInfo.Dtos;
using PersonalSite.Application.Features.Common.SocialMediaLinks.Dtos;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.SiteInfo.Queries.GetSiteInfo;

public class GetSiteInfoHandler : IRequestHandler<GetSiteInfoQuery, Result<SiteInfoDto>>
{
    private readonly ILanguageRepository _languageRepository;
    private readonly ISocialMediaLinkRepository _socialMediaLinkRepository;
    private readonly IResumeRepository _resumeRepository;
    private readonly ILogger<GetSiteInfoHandler> _logger;
    private readonly IMapper<Domain.Entities.Common.Language, LanguageDto> _languageMapper;
    private readonly IMapper<SocialMediaLink, SocialMediaLinkDto> _socialMediaLinkMapper;
    private readonly IMapper<Domain.Entities.Common.Resume, ResumeDto> _resumeMapper;

    public GetSiteInfoHandler(
        ILanguageRepository languageRepository,
        ISocialMediaLinkRepository socialMediaLinkRepository,
        IResumeRepository resumeRepository,
        ILogger<GetSiteInfoHandler> logger,
        IMapper<Domain.Entities.Common.Language, LanguageDto> languageMapper,
        IMapper<SocialMediaLink, SocialMediaLinkDto> socialMediaLinkMapper,
        IMapper<Domain.Entities.Common.Resume, ResumeDto> resumeMapper)
    {
        _languageRepository = languageRepository;
        _socialMediaLinkRepository = socialMediaLinkRepository;
        _resumeRepository = resumeRepository;
        _logger = logger;
        _languageMapper = languageMapper;
        _socialMediaLinkMapper = socialMediaLinkMapper;
        _resumeMapper = resumeMapper;   
    }
    
    public async Task<Result<SiteInfoDto>> Handle(GetSiteInfoQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var languages = await _languageRepository.ListAsync(cancellationToken);
            if (languages.Count < 1)
            {
                _logger.LogWarning("No languages found."); 
                return Result<SiteInfoDto>.Failure("No languages found.");
            }
            var languagesData = _languageMapper.MapToDtoList(languages.Where(l => !l.IsDeleted));
        
            var socialLinks = await _socialMediaLinkRepository.GetAllActiveAsync(cancellationToken);
            if (socialLinks.Count < 1)
            {
                _logger.LogWarning("No social links found.");     
            }
            var socialLinksData = _socialMediaLinkMapper.MapToDtoList(socialLinks);

            var resume = await _resumeRepository.GetLastActiveAsync(cancellationToken);
            if (resume is null)
            {
                _logger.LogWarning("No active resume found.");     
            }
            var resumeData = resume is null ? null : _resumeMapper.MapToDto(resume);
        
            var siteInfo = new SiteInfoDto
            {
                Languages = languagesData,
                SocialLinks = socialLinksData,
                Resume = resumeData
            };
        
            return Result<SiteInfoDto>.Success(siteInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while getting site info.");
            return Result<SiteInfoDto>.Failure("An unexpected error occurred.");
        }
    }
}