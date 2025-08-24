using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Interfaces.Repositories.Blog;

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
            .AsSplitQuery()
            .ToListAsync(cancellationToken);
    }
}
