using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Pages;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetPrivacyPage;

public class GetPrivacyPageQueryHandler : IRequestHandler<GetPrivacyPageQuery, Result<PrivacyPageDto>>
{
    private const string Key = "privacy";
    private readonly LanguageContext _language;
    private readonly IPageRepository _pageRepository;
    private readonly ILogger<GetPrivacyPageQueryHandler> _logger;
    private readonly ITranslatableMapper<Domain.Entities.Pages.Page, PageDto> _pageMapper;
    
    public GetPrivacyPageQueryHandler(
        LanguageContext language,
        IPageRepository pageRepository,
        ILogger<GetPrivacyPageQueryHandler> logger,
        ITranslatableMapper<Domain.Entities.Pages.Page, PageDto> pageMapper)
    {
        _language = language;
        _pageRepository = pageRepository;
        _logger = logger;
        _pageMapper = pageMapper; 
    }
    
    public async Task<Result<PrivacyPageDto>> Handle(GetPrivacyPageQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_language.LanguageCode))
            {
                return Result<PrivacyPageDto>.Failure("Invalid language context.");
            }
        
            var page = await _pageRepository.GetByKeyAsync(Key, cancellationToken);
            if (page == null)
            {
                _logger.LogWarning("Privacy page not found.");
                return Result<PrivacyPageDto>.Failure("Privacy page not found.");
            }
            var pageData = _pageMapper.MapToDto(page, _language.LanguageCode);
        
            var cookiesPage = new PrivacyPageDto
            {
                PageData = pageData
            };
        
            return Result<PrivacyPageDto>.Success(cookiesPage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving privacy page data.");
            return Result<PrivacyPageDto>.Failure("An unexpected error occurred.");
        }
    }
}