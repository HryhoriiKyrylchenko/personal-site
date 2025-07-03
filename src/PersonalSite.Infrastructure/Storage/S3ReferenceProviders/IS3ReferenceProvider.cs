namespace PersonalSite.Infrastructure.Storage.S3ReferenceProviders;

public interface IS3ReferenceProvider
{
    Task<IEnumerable<string>> GetUsedS3UrlsAsync(CancellationToken cancellationToken);
}