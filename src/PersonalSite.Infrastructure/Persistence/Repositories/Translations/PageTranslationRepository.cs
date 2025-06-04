namespace PersonalSite.Infrastructure.Persistence.Repositories.Translations;

public class PageTranslationRepository : EfRepository<PageTranslation>, IPageTranslationRepository
{
    public PageTranslationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<PageTranslation?> GetByPageKeyAndLanguageAsync(string pageKey, string languageCode, CancellationToken cancellationToken = default)
    {
        return await DbContext.PageTranslations
            .FirstOrDefaultAsync(pt => pt.PageKey == pageKey && pt.LanguageCode == languageCode, cancellationToken);
    }
}