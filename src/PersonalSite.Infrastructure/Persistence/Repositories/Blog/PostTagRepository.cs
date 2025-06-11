namespace PersonalSite.Infrastructure.Persistence.Repositories.Blog;

public class PostTagRepository : EfRepository<PostTag>, IPostTagRepository
{
    public PostTagRepository(
        ApplicationDbContext context, 
        ILogger<PostTagRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<List<PostTag>> GetByBlogPostIdAsync(Guid blogPostId, CancellationToken cancellationToken = default)
    {
        return await DbContext.PostTags
            .Include(pt => pt.BlogPostTag)
            .Where(pt => pt.BlogPostId == blogPostId)
            .ToListAsync(cancellationToken);
    }

    public async Task<PostTag?> GetByBlogPostAndTagAsync(Guid blogPostId, Guid blogPostTagId, CancellationToken cancellationToken = default)
    {
        return await DbContext.PostTags
            .FirstOrDefaultAsync(pt => pt.BlogPostId == blogPostId && pt.BlogPostTagId == blogPostTagId, cancellationToken);
    }
}
