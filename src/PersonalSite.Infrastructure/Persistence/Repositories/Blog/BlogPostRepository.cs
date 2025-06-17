namespace PersonalSite.Infrastructure.Persistence.Repositories.Blog;

public class BlogPostRepository : EfRepository<BlogPost>, IBlogPostRepository
{
    public BlogPostRepository(
        ApplicationDbContext context, 
        ILogger<BlogPostRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<BlogPost?> GetByIdWithTagsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        
        return await DbContext.BlogPosts
            .Where(p => !p.IsDeleted)
            .Include(p => p.Translations)
                .ThenInclude(t => t.Language)
            .Include(p => p.PostTags)
                .ThenInclude(pt => pt.BlogPostTag)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<BlogPost?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await DbContext.BlogPosts
            .Include(p => p.Translations)
                .ThenInclude(t => t.Language)
            .Include(p => p.PostTags)
                .ThenInclude(pt => pt.BlogPostTag)
            .FirstOrDefaultAsync(p => p.Slug == slug, cancellationToken);
    }

    public async Task<IReadOnlyList<BlogPost>> GetPublishedPostsAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.BlogPosts
            .Where(p => p.IsPublished && !p.IsDeleted)
            .Include(p => p.Translations)
                .ThenInclude(t => t.Language)
            .Include(p => p.PostTags)
                .ThenInclude(pt => pt.BlogPostTag)
            .OrderByDescending(p => p.PublishedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<BlogPost>> GetAllWithTagsAsync(CancellationToken cancellationToken)
    {
        return await DbContext.BlogPosts
            .Where(p => !p.IsDeleted)
            .Include(p => p.Translations)
                .ThenInclude(t => t.Language)
            .Include(p => p.PostTags)
                .ThenInclude(pt => pt.BlogPostTag)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}