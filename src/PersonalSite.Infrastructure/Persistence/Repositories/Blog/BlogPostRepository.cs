namespace PersonalSite.Infrastructure.Persistence.Repositories.Blog;

public class BlogPostRepository : EfRepository<BlogPost>, IBlogPostRepository
{
    public BlogPostRepository(ApplicationDbContext dbContext) : base(dbContext) {}
    
    public async Task<BlogPost?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await DbContext.BlogPosts
            .Include(p => p.Translations)
            .FirstOrDefaultAsync(p => p.Slug == slug, cancellationToken);
    }

    public async Task<IReadOnlyList<BlogPost>> GetPublishedPostsAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.BlogPosts
            .Where(p => p.IsPublished)
            .OrderByDescending(p => p.PublishedAt)
            .ToListAsync(cancellationToken);
    }
}