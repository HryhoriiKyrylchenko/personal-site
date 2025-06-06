namespace PersonalSite.Application.Services.Translations;

public class PageTranslationService : 
    CrudServiceBase<PageTranslation, PageTranslationDto, PageTranslationAddRequest, PageTranslationUpdateRequest>, 
    IPageTranslationService
{
    private readonly LanguageContext _language;
    private readonly IPageTranslationRepository _pageTranslationRepository;
    
    public PageTranslationService(
        IPageTranslationRepository repository, 
        IUnitOfWork unitOfWork,
        LanguageContext language) 
        : base(repository, unitOfWork)
    {
        _language = language;
        _pageTranslationRepository = repository;
    }

    public async Task<PageTranslationDto?> GetPageByKeyAsync(string pageKey, CancellationToken cancellationToken = default)
    {
        var page = await _pageTranslationRepository.GetByPageKeyAndLanguageAsync(pageKey, _language.LanguageCode, cancellationToken);
        
        return page == null ? null : EntityToDtoMapper.MapPageTranslationToDto(page);
    }

    public override async Task<PageTranslationDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task<IReadOnlyList<PageTranslationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task AddAsync(PageTranslationAddRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(PageTranslationUpdateRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}