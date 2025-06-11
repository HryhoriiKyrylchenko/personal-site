namespace PersonalSite.Infrastructure.Persistence.Repositories.Translations;

public class PageTranslationRepository : EfRepository<PageTranslation>, IPageTranslationRepository
{
    public PageTranslationRepository(
        ApplicationDbContext context, 
        ILogger<PageTranslationRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<List<PageTranslation>> GetAllByPageKeyAsync(string pageKey, CancellationToken cancellationToken = default)
    {
        return await DbContext.PageTranslations
            .Where(p => p.Page.Key == pageKey)
            .ToListAsync(cancellationToken);
    }

    public async Task<PageTranslation?> GetWithLanguageByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return await DbContext.PageTranslations
            .Include(t => t.Language)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<PageTranslation>> ListWithLanguageAsync(CancellationToken cancellationToken)
    {
        return await DbContext.PageTranslations
            .Include(t => t.Language)
            .ToListAsync(cancellationToken);
    }
}