namespace PersonalSite.Infrastructure.Persistence.Repositories.Blog;

public class BlogPostRepository : EfRepository<BlogPost>, IBlogPostRepository
{
    public BlogPostRepository(ApplicationDbContext dbContext) : base(dbContext) {}

    public async Task<BlogPost?> GetByIdWithTagsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await DbContext.BlogPosts
            .Where(p => !p.IsDeleted)
            .Include(p => p.Translations)
            .Include(p => p.PostTags)
                .ThenInclude(pt => pt.BlogPostTag)
            .FirstOrDefaultAsync(p => p.Id == id, cancellationToken);
    }

    public async Task<BlogPost?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await DbContext.BlogPosts
            .Include(p => p.Translations)
            .Include(p => p.PostTags)
                .ThenInclude(pt => pt.BlogPostTag)
            .FirstOrDefaultAsync(p => p.Slug == slug, cancellationToken);
    }

    public async Task<IReadOnlyList<BlogPost>> GetPublishedPostsAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.BlogPosts
            .Where(p => p.IsPublished && !p.IsDeleted)
            .Include(p => p.Translations)
            .Include(p => p.PostTags)
                .ThenInclude(pt => pt.BlogPostTag)
            .OrderByDescending(p => p.PublishedAt)
            .ToListAsync(cancellationToken);
    }

    public async Task<IEnumerable<BlogPost>> GetAllWithTagsAsync(CancellationToken cancellationToken)
    {
        return await DbContext.BlogPosts
            .Where(p => !p.IsDeleted)
            .Include(p => p.Translations)
            .Include(p => p.PostTags)
                .ThenInclude(pt => pt.BlogPostTag)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}