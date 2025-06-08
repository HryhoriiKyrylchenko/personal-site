namespace PersonalSite.Application.Services.Aggregates;

public class SiteInfoService : ISiteInfoService
{
    private readonly ILanguageService _languageService;
    private readonly ISocialMediaLinkService _socialService;
    private readonly IResumeService _resumeService;
    
    public SiteInfoService(
        ILanguageService languageService,
        ISocialMediaLinkService socialService,
        IResumeService resumeService)
    {
        _languageService = languageService;
        _socialService = socialService;
        _resumeService = resumeService;
    }
    
    public async Task<SiteInfoDto?> GetAsync(CancellationToken cancellationToken = default)
    {
        var languages = await _languageService.GetAllAsync(cancellationToken);
        var socialLinks = await _socialService.GetAllAsync(cancellationToken);
        var resume = await _resumeService.GetLatestAsync(cancellationToken);
        
        return new SiteInfoDto
        {
            Languages = languages,
            SocialLinks = socialLinks,
            Resume = resume
        };
    }
}