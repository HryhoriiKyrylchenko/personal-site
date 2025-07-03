namespace PersonalSite.Infrastructure.Storage.S3ReferenceProviders;

public class ProjectS3ReferenceProvider : IS3ReferenceProvider
{
    private readonly ApplicationDbContext _dbContext;

    public ProjectS3ReferenceProvider(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<string>> GetUsedS3UrlsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Projects
            .Where(bp => !string.IsNullOrEmpty(bp.CoverImage))
            .Select(bp => bp.CoverImage)
            .ToListAsync(cancellationToken);
    }
}