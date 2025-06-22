namespace PersonalSite.Infrastructure.BackgroundProcessing.OrphanFileCleanupJob;

public class S3OrphanFileCleanupJob : IHostedService, IDisposable
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<S3OrphanFileCleanupJob> _logger;
    private readonly IAmazonS3 _s3Client;
    private readonly AwsS3Settings _settings;
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
        }, null, TimeSpan.FromMinutes(1), TimeSpan.FromHours(12));
        return Task.CompletedTask;
    }

    private async Task DoCleanupAsync()
    {
        try
        {
            _logger.LogInformation("Running S3 orphan file cleanup...");

            var listedObjects = await _s3Client.ListObjectsV2Async(new ListObjectsV2Request
            {
                BucketName = _settings.BucketName,
                Prefix = "uploads/"
            });

            var s3Keys = listedObjects.S3Objects
                .Where(o => o.LastModified < DateTime.UtcNow.AddDays(-1))
                .Select(o => o.Key)
                .ToList();

            using var scope = _scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

            var dbUrls = await dbContext.BlogPosts
                .Select(bp => bp.CoverImage)
                .Concat(dbContext.Projects.Select(p => p.CoverImage))
                .Where(url => true)
                .ToListAsync();

            var dbKeys = dbUrls
                .Select(url => url.Split(".amazonaws.com/").LastOrDefault())
                .Where(key => key != null)
                .ToHashSet();

            var orphanKeys = s3Keys.Except(dbKeys).ToList();

            foreach (var orphanKey in orphanKeys)
            {
                await _s3Client.DeleteObjectAsync(_settings.BucketName, orphanKey);
                _logger.LogInformation("Deleted orphaned S3 file: {Key}", orphanKey);
            }

            _logger.LogInformation("S3 orphan file cleanup complete.");
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