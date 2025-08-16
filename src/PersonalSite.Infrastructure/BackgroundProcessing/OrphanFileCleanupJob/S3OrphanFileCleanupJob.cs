using PersonalSite.Infrastructure.Storage.S3ReferenceProviders;

namespace PersonalSite.Infrastructure.BackgroundProcessing.OrphanFileCleanupJob;

public class S3OrphanFileCleanupJob : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IAmazonS3 _s3Client;
    private readonly AwsS3Settings _settings;
    private readonly ILogger<S3OrphanFileCleanupJob> _logger;
    private Timer? _timer;

    public S3OrphanFileCleanupJob(
        IServiceScopeFactory scopeFactory,
        IAmazonS3 s3Client,
        IOptions<AwsS3Settings> options,
        ILogger<S3OrphanFileCleanupJob> logger)
    {
        _scopeFactory = scopeFactory;
        _s3Client = s3Client;
        _settings = options.Value;
        _logger = logger;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _timer = new Timer(async void (_) =>
        {
            try
            {
                await DoCleanupAsync();
            }
            catch (Exception e)
            {
               _logger.LogError(e, "Error during S3 orphan file cleanup.");
            }
        }, null, TimeSpan.FromMinutes(5), TimeSpan.FromDays(7));
        return Task.CompletedTask;
    }

    private async Task DoCleanupAsync()
    {
        try
        {
            _logger.LogInformation("Running S3 orphan file cleanup...");

            using var scope = _scopeFactory.CreateScope();
            var referenceProviders = scope.ServiceProvider.GetServices<IS3ReferenceProvider>();

            var usedKeys = new HashSet<string>();

            foreach (var provider in referenceProviders)
            {
                var urls = await provider.GetUsedS3UrlsAsync(CancellationToken.None);
                foreach (var url in urls)
                    usedKeys.Add(url);
            }

            var listedObjects = await _s3Client.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = _settings.BucketName
            });

            var s3Keys = listedObjects.S3Objects?
                             .Where(o => o.LastModified < DateTime.UtcNow.AddDays(-1))
                             .Select(o => o.Key)
                             .ToList()
                         ?? [];

            var orphanKeys = s3Keys.Except(usedKeys).ToList();

            foreach (var orphanKey in orphanKeys)
            {
                await _s3Client.DeleteObjectAsync(_settings.BucketName, orphanKey);
                _logger.LogInformation("Deleted orphaned S3 file: {Key}", orphanKey);
            }

            _logger.LogInformation("S3 orphan cleanup complete: deleted {Count} files", orphanKeys.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error during S3 orphan file cleanup.");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _timer?.Dispose();
        return Task.CompletedTask;
    }

    public void Dispose() => _timer?.Dispose();
}