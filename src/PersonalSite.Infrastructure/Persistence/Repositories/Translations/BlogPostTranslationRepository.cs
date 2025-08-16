using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

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
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }
}