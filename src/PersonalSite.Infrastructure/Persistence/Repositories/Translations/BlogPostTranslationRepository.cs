namespace PersonalSite.Infrastructure.Persistence.Repositories.Translations;

public class BlogPostTranslationRepository : EfRepository<BlogPostTranslation>, IBlogPostTranslationRepository
{
    public BlogPostTranslationRepository(
        ApplicationDbContext context, 
        ILogger<BlogPostTranslationRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<List<BlogPostTranslation>> GetByBlogPostIdAsync(Guid blogPostId, CancellationToken cancellationToken = default)
    {
        if (blogPostId == Guid.Empty)
            throw new ArgumentException("Blog post ID cannot be empty", nameof(blogPostId));
        
        return await DbContext.BlogPostTranslations
            .Where(t => t.BlogPostId == blogPostId)
            .ToListAsync(cancellationToken);
    }

    public async Task<BlogPostTranslation?> GetByBlogPostIdAndLanguageAsync(Guid blogPostId, string languageCode, CancellationToken cancellationToken = default)
    {
        if (blogPostId == Guid.Empty)
            throw new ArgumentException("Blog post ID cannot be empty", nameof(blogPostId));
        
        if (string.IsNullOrWhiteSpace(languageCode))
            throw new ArgumentException("Language code cannot be null or whitespace", nameof(languageCode));
        
        return await DbContext.BlogPostTranslations
            .FirstOrDefaultAsync(t => t.BlogPostId == blogPostId && t.Language.Code == languageCode, cancellationToken);
    }

    public async Task<BlogPostTranslation?> GetWithLanguageByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        
        return await DbContext.BlogPostTranslations
            .Include(t => t.Language)
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<IEnumerable<BlogPostTranslation>> ListWithLanguageAsync(CancellationToken cancellationToken)
    {
        return await DbContext.BlogPostTranslations
            .Include(t => t.Language)
            .ToListAsync(cancellationToken);
    }
}