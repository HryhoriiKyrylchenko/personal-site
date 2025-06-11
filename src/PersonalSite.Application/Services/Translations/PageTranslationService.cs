namespace PersonalSite.Application.Services.Translations;

public class PageTranslationService : 
    CrudServiceBase<PageTranslation, PageTranslationDto, PageTranslationAddRequest, PageTranslationUpdateRequest>, 
    IPageTranslationService
{
    private readonly IPageTranslationRepository _pageTranslationRepository;
    
    public PageTranslationService(
        IPageTranslationRepository repository, 
        IUnitOfWork unitOfWork) 
        : base(repository, unitOfWork)
    {
        _pageTranslationRepository = repository;
    }

    public async Task<List<PageTranslationDto>> GetAllByPageKeyAsync(string pageKey, CancellationToken cancellationToken = default)
    {
        var pages = await _pageTranslationRepository.GetAllByPageKeyAsync(pageKey, cancellationToken);

        return EntityToDtoMapper.MapPageTranslationsToDtoList(pages);
    }

    public override async Task<PageTranslationDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var pageTranslation = await _pageTranslationRepository.GetWithLanguageByIdAsync(id, cancellationToken);
        return pageTranslation == null
            ? null
            : EntityToDtoMapper.MapPageTranslationToDto(pageTranslation);
    }

    public override async Task<IReadOnlyList<PageTranslationDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var pageTranslations = await _pageTranslationRepository.ListWithLanguageAsync(cancellationToken);
        
        return EntityToDtoMapper.MapPageTranslationsToDtoList(pageTranslations);
    }

    public override async Task AddAsync(PageTranslationAddRequest request, CancellationToken cancellationToken = default)
    {
        var newPageTranslation = new PageTranslation
        {
            Id = Guid.NewGuid(),
            LanguageId = request.LanguageId,
            PageId = request.PageId,
            Data = request.Data,
            Title = request.Title,
            Description = request.Description,
            MetaTitle = request.MetaTitle,
            MetaDescription = request.MetaDescription,
            OgImage = request.OgImage
        };
        
        await _pageTranslationRepository.AddAsync(newPageTranslation, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(PageTranslationUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var existingPageTranslation = await _pageTranslationRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingPageTranslation is null) throw new Exception("Page translation not found");
        
        existingPageTranslation.Data = request.Data;
        existingPageTranslation.Title = request.Title;
        existingPageTranslation.Description = request.Description;
        existingPageTranslation.MetaTitle = request.MetaTitle;
        existingPageTranslation.MetaDescription = request.MetaDescription;
        existingPageTranslation.OgImage = request.OgImage;
        
        await _pageTranslationRepository.UpdateAsync(existingPageTranslation, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await _pageTranslationRepository.GetByIdAsync(id, cancellationToken);
        if (entity is not null)
        {
            _pageTranslationRepository.Remove(entity);
            await UnitOfWork.SaveChangesAsync(cancellationToken);
        }  
    }
}