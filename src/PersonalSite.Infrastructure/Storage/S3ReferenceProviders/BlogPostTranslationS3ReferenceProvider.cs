namespace PersonalSite.Infrastructure.Storage.S3ReferenceProviders;

public class BlogPostTranslationS3ReferenceProvider : IS3ReferenceProvider 
{
    private readonly ApplicationDbContext _dbContext;

    public BlogPostTranslationS3ReferenceProvider(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<string>> GetUsedS3UrlsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.BlogPostTranslations
            .Where(bp => !string.IsNullOrEmpty(bp.OgImage))
            .Select(bp => bp.OgImage)
            .ToListAsync(cancellationToken);
    }
}