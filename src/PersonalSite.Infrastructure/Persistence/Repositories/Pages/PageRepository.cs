namespace PersonalSite.Infrastructure.Persistence.Repositories.Pages;

public class PageRepository : EfRepository<Page>, IPageRepository
{
    public PageRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Page?> GetByKeyAsync(string key, CancellationToken cancellationToken = default)
    {
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
        return await DbContext.Pages
            .Include(p => p.Translations)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }
}