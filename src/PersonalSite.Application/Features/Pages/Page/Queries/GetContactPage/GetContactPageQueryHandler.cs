using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Pages;

namespace PersonalSite.Application.Features.Pages.Page.Queries.GetContactPage;

public class GetContactPageQueryHandler : IRequestHandler<GetContactPageQuery, Result<ContactPageDto>>
{
    private const string Key = "contacts";
    private readonly LanguageContext _language;
    private readonly IPageRepository _pageRepository;
    private readonly ILogger<GetContactPageQueryHandler> _logger;
    private readonly ITranslatableMapper<Domain.Entities.Pages.Page, PageDto> _pageMapper;
    
    public GetContactPageQueryHandler(
        LanguageContext language,
        IPageRepository pageRepository,
        ILogger<GetContactPageQueryHandler> logger,
        ITranslatableMapper<Domain.Entities.Pages.Page, PageDto> pageMapper)
    {
        _language = language;
        _pageRepository = pageRepository;
        _logger = logger;
        _pageMapper = pageMapper; 
    }
    
    public async Task<Result<ContactPageDto>> Handle(GetContactPageQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(_language.LanguageCode))
            {
                return Result<ContactPageDto>.Failure("Invalid language context.");
            }
        
            var page = await _pageRepository.GetByKeyAsync(Key, cancellationToken);
            if (page == null)
            {
                _logger.LogWarning("Contact page not found.");
                return Result<ContactPageDto>.Failure("Contact page not found.");
            }
            var pageData = _pageMapper.MapToDto(page, _language.LanguageCode);
        
            var contactPage = new ContactPageDto
            {
                PageData = pageData
            };
        
            return Result<ContactPageDto>.Success(contactPage);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while retrieving contact page data.");
            return Result<ContactPageDto>.Failure("An unexpected error occurred.");
        }
    }
}