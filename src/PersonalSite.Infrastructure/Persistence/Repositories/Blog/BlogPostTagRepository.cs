namespace PersonalSite.Infrastructure.Persistence.Repositories.Blog;

public class BlogPostTagRepository : EfRepository<BlogPostTag>, IBlogPostTagRepository
{
    public BlogPostTagRepository(ApplicationDbContext context) : base(context) { }

    public async Task<BlogPostTag?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbContext.BlogPostTags
            .FirstOrDefaultAsync(t => t.Name == name, cancellationToken);
    }

    public async Task<List<BlogPostTag>> GetAllSortedAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.BlogPostTags
            .OrderBy(t => t.Name)
            .ToListAsync(cancellationToken);
    }
}