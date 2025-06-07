namespace PersonalSite.Infrastructure.Persistence.Repositories.Translations;

public class PageTranslationRepository : EfRepository<PageTranslation>, IPageTranslationRepository
{
    public PageTranslationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<PageTranslation>> GetAllByPageKeyAsync(string pageKey, CancellationToken cancellationToken = default)
    {
        return await DbContext.PageTranslations
            .Where(p => p.Page.Key == pageKey)
            .ToListAsync(cancellationToken);
    }
}