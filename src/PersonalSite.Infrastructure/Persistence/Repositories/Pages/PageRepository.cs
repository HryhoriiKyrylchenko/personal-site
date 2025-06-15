namespace PersonalSite.Infrastructure.Persistence.Repositories.Pages;

public class PageRepository : EfRepository<Page>, IPageRepository
{
    public PageRepository(
        ApplicationDbContext context, 
        ILogger<PageRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<Page?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(key))
            throw new ArgumentException("Key cannot be null or whitespace", nameof(key));
        
        return await DbContext.Pages
            .Include(p => p.Translations)
            .FirstOrDefaultAsync(p => p.Key == key, cancellationToken);
    }

    public async Task<List<Page>> GetAllWithTranslationsAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.Pages
            .Include(p => p.Translations)
            .ToListAsync(cancellationToken);
    }

    public async Task<Page?> GetWithTranslationByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        
        return await DbContext.Pages
            .Include(p => p.Translations)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}