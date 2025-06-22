using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

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
        if (string.IsNullOrWhiteSpace(pageKey))
            throw new ArgumentException("Page key cannot be null or whitespace", nameof(pageKey));
        
        return await DbContext.PageTranslations
            .Where(p => p.Page.Key == pageKey)
            .ToListAsync(cancellationToken);
    }
}