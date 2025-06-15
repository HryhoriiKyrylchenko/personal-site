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
        if (blogPostId == Guid.Empty)
            throw new ArgumentException("Blog post ID cannot be empty", nameof(blogPostId));
        
        return await DbContext.PostTags
            .Include(pt => pt.BlogPostTag)
            .Where(pt => pt.BlogPostId == blogPostId)
            .ToListAsync(cancellationToken);
    }

    public async Task<PostTag?> GetByBlogPostAndTagAsync(Guid blogPostId, Guid blogPostTagId, CancellationToken cancellationToken = default)
    {
        if (blogPostId == Guid.Empty)
            throw new ArgumentException("Blog post ID cannot be empty", nameof(blogPostId));
        
        if (blogPostTagId == Guid.Empty)
            throw new ArgumentException("Blog post tag ID cannot be empty", nameof(blogPostTagId));
        
        return await DbContext.PostTags
            .FirstOrDefaultAsync(pt => pt.BlogPostId == blogPostId && pt.BlogPostTagId == blogPostTagId, cancellationToken);
    }
}
