using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Pages;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetCookiesPage;

public class GetCookiesPageQueryHandler : IRequestHandler<GetCookiesPageQuery, Result<CookiesPageDto>>
{
    private const string Key = "cookies";
    private readonly LanguageContext _language;
    private readonly IPageRepository _pageRepository;
    private readonly ILogger<GetCookiesPageQueryHandler> _logger;
    private readonly ITranslatableMapper<Domain.Entities.Pages.Page, PageDto> _pageMapper;
    
    public GetCookiesPageQueryHandler(
        LanguageContext language,
        IPageRepository pageRepository,
        ILogger<GetCookiesPageQueryHandler> logger,
        ITranslatableMapper<Domain.Entities.Pages.Page, PageDto> pageMapper)
    {
        _language = language;
        _pageRepository = pageRepository;
        _logger = logger;
        _pageMapper = pageMapper; 
    }
    
    public async Task<Result<CookiesPageDto>> Handle(GetCookiesPageQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_language.LanguageCode))
            {
                return Result<CookiesPageDto>.Failure("Invalid language context.");
            }
        
            var page = await _pageRepository.GetByKeyAsync(Key, cancellationToken);
            if (page == null)
            {
                _logger.LogWarning("Cookies page not found.");
                return Result<CookiesPageDto>.Failure("Cookies page not found.");
            }
            var pageData = _pageMapper.MapToDto(page, _language.LanguageCode);
        
            var cookiesPage = new CookiesPageDto
            {
                PageData = pageData
            };
        
            return Result<CookiesPageDto>.Success(cookiesPage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving cookies page data.");
            return Result<CookiesPageDto>.Failure("An unexpected error occurred.");
        }
    }
}