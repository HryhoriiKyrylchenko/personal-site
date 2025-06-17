namespace PersonalSite.Application.Features.Common.SiteInfo.Queries.GetSiteInfo;

public class GetSiteInfoHandler : IRequestHandler<GetSiteInfoQuery, Result<SiteInfoDto>>
{
    private readonly ILanguageRepository _languageRepository;
    private readonly ISocialMediaLinkRepository _socialMediaLinkRepository;
    private readonly IResumeRepository _resumeRepository;
    private readonly ILogger<GetSiteInfoHandler> _logger;

    public GetSiteInfoHandler(
        ILanguageRepository languageRepository,
        ISocialMediaLinkRepository socialMediaLinkRepository,
        IResumeRepository resumeRepository,
        ILogger<GetSiteInfoHandler> logger)
    {
        _languageRepository = languageRepository;
        _socialMediaLinkRepository = socialMediaLinkRepository;
        _resumeRepository = resumeRepository;
        _logger = logger;
    }
    
    public async Task<Result<SiteInfoDto>> Handle(GetSiteInfoQuery request, CancellationToken cancellationToken)
    {
        var languages = await _languageRepository.ListAsync(cancellationToken);
        if (languages.Count < 1)
        {
            _logger.LogWarning("No languages found."); 
            return Result<SiteInfoDto>.Failure("No languages found.");
        }
        var languagesData = EntityToDtoMapper.MapLanguagesToDtoList(languages);
        
        var socialLinks = await _socialMediaLinkRepository.GetAllActiveAsync(cancellationToken);
        if (socialLinks.Count < 1)
        {
            _logger.LogWarning("No social links found.");     
        }
        var socialLinksData = EntityToDtoMapper.MapSocialMediaLinksToDtoList(socialLinks);

        var resume = await _resumeRepository.GetLastActiveAsync(cancellationToken);
        if (resume is null)
        {
            _logger.LogWarning("No active resume found.");     
        }
        var resumeData = resume is null ? null : EntityToDtoMapper.MapResumeToDto(resume);
        
        var siteInfo = new SiteInfoDto
        {
            Languages = languagesData,
            SocialLinks = socialLinksData,
            Resume = resumeData
        };
        
        return Result<SiteInfoDto>.Success(siteInfo);
    }
}