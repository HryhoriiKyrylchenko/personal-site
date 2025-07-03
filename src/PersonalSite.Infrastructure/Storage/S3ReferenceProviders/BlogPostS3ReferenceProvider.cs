namespace PersonalSite.Infrastructure.Storage.S3ReferenceProviders;

public class BlogPostS3ReferenceProvider : IS3ReferenceProvider
{
    private readonly ApplicationDbContext _dbContext;

    public BlogPostS3ReferenceProvider(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<string>> GetUsedS3UrlsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.BlogPosts
            .Where(bp => !string.IsNullOrEmpty(bp.CoverImage))
            .Select(bp => bp.CoverImage)
            .ToListAsync(cancellationToken);
    }
}
