using PersonalSite.Domain.Entities.Blog;
using PersonalSite.Domain.Interfaces.Repositories.Blog;

namespace PersonalSite.Infrastructure.Persistence.Repositories.Blog;

public class BlogPostTagRepository : EfRepository<BlogPostTag>, IBlogPostTagRepository
{
    public BlogPostTagRepository(
        ApplicationDbContext context, 
        ILogger<BlogPostTagRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<BlogPostTag?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name cannot be null or whitespace", nameof(name));
        
        return await DbContext.BlogPostTags
            .FirstOrDefaultAsync(t => t.Name == name, cancellationToken);
    }
}