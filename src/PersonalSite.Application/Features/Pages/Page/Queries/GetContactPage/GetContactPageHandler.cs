namespace PersonalSite.Application.Features.Pages.Page.Queries.GetContactPage;

public class GetContactPageHandler : IRequestHandler<GetContactPageQuery, Result<ContactPageDto>>
{
    private const string Key = "contacts";
    private readonly LanguageContext _language;
    private readonly IPageRepository _pageRepository;
    private readonly ILogger<GetContactPageHandler> _logger;
    
    public GetContactPageHandler(
        LanguageContext language,
        IPageRepository pageRepository,
        IBlogPostRepository blogPostRepository,
        ILogger<GetContactPageHandler> logger)
    {
        _language = language;
        _pageRepository = pageRepository;
        _logger = logger;
    }
    
    public async Task<Result<ContactPageDto>> Handle(GetContactPageQuery request, CancellationToken cancellationToken)
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
        var pageData = EntityToDtoMapper.MapPageToDto(page, _language.LanguageCode);
        
        var contactPage = new ContactPageDto
        {
            PageData = pageData
        };
        
        return Result<ContactPageDto>.Success(contactPage);
    }
}