using PersonalSite.Domain.Interfaces.Repositories.Pages;

namespace PersonalSite.Application.Services.Pages;

public class PageService : 
    CrudServiceBase<Page, PageDto, PageAddRequest, PageUpdateRequest>, 
    IPageService
{
    private readonly LanguageContext _language;
    private readonly IPageRepository _pageRepository;
    
    public PageService(
        IPageRepository repository, 
        IUnitOfWork unitOfWork,
        LanguageContext language) 
        : base(repository, unitOfWork)
    {
        _pageRepository = repository;
        _language = language;
    }

    public override async Task<PageDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var page = await _pageRepository.GetWithTranslationByIdAsync(id, cancellationToken);
        
        return page == null ? null : EntityToDtoMapper.MapPageToDto(page, _language.LanguageCode);
    }

    public override async Task<IReadOnlyList<PageDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var pages = await _pageRepository.GetAllWithTranslationsAsync(cancellationToken);
        
        return EntityToDtoMapper.MapPagesToDtoList(pages, _language.LanguageCode);   
    }

    public override async Task AddAsync(PageAddRequest request, CancellationToken cancellationToken = default)
    {
        var newPage = new Page()
        {
            Id = Guid.NewGuid(),
            Key = request.Key
        };
        
        await _pageRepository.AddAsync(newPage, cancellationToken);
        await UnitOfWork.SaveChangesAsync(cancellationToken);
    }

    public override async Task UpdateAsync(PageUpdateRequest request, CancellationToken cancellationToken = default)
    {
        var existingPage = await _pageRepository.GetByIdAsync(request.Id, cancellationToken);
        if (existingPage is null) throw new Exception("Page not found");
        
        existingPage.Key = request.Key;
        
        _pageRepository.Update(existingPage);
        await UnitOfWork.SaveChangesAsync(cancellationToken); 
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        if (entity is not null)
        {
            Repository.Remove(entity);
            await UnitOfWork.SaveChangesAsync(cancellationToken);
        } 
    }

    public async Task<PageDto?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        var page = await _pageRepository.GetByKeyAsync(key, cancellationToken);
        
        return page == null ? null : EntityToDtoMapper.MapPageToDto(page, _language.LanguageCode);
    }
}