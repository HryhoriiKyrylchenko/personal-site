namespace PersonalSite.Infrastructure.Storage.S3ReferenceProviders;

public class PageTranslationS3ReferenceProvider : IS3ReferenceProvider 
{
    private readonly ApplicationDbContext _dbContext;

    public PageTranslationS3ReferenceProvider(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IEnumerable<string>> GetUsedS3UrlsAsync(CancellationToken cancellationToken)
    {
        List<string> urls = [];
        
        var ogImages = await _dbContext.PageTranslations
            .Where(bp => !string.IsNullOrEmpty(bp.OgImage))
            .Select(bp => bp.OgImage)
            .ToListAsync(cancellationToken);

        urls.AddRange(ogImages);
        
        var pageTranslationsWithData = await _dbContext.PageTranslations
            .ToListAsync(cancellationToken);
        
        foreach (var pt in pageTranslationsWithData)
        {
            if (pt.Data.TryGetValue("ImageUrl", out var imageUrl))
            {
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    urls.Add(imageUrl);
                }
            }
        }
        
        return urls;
    }
}