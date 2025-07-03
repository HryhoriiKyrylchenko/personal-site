namespace PersonalSite.Infrastructure.Storage.S3ReferenceProviders;

public class ResumeS3ReferenceProvider : IS3ReferenceProvider
{
    private readonly ApplicationDbContext _dbContext;

    public ResumeS3ReferenceProvider(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<string>> GetUsedS3UrlsAsync(CancellationToken cancellationToken)
    {
        return await _dbContext.Resumes
            .Where(r => !string.IsNullOrEmpty(r.FileUrl))
            .Select(r => r.FileUrl)
            .ToListAsync(cancellationToken);
    }
}