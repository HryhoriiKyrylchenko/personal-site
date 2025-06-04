namespace PersonalSite.Infrastructure.Persistence.Repositories.Translations;

public class BlogPostTranslationRepository : EfRepository<BlogPostTranslation>, IBlogPostTranslationRepository
{
    public BlogPostTranslationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<BlogPostTranslation>> GetByBlogPostIdAsync(Guid blogPostId, CancellationToken cancellationToken = default)
    {
        return await DbContext.BlogPostTranslations
            .Where(t => t.BlogPostId == blogPostId)
            .ToListAsync(cancellationToken);
    }

    public async Task<BlogPostTranslation?> GetByBlogPostIdAndLanguageAsync(Guid blogPostId, string languageCode, CancellationToken cancellationToken = default)
    {
        return await DbContext.BlogPostTranslations
            .FirstOrDefaultAsync(t => t.BlogPostId == blogPostId && t.LanguageCode == languageCode, cancellationToken);
    }
}