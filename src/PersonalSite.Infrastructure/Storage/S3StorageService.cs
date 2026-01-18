namespace PersonalSite.Infrastructure.Storage;

public class S3StorageService : IStorageService
{
    private readonly IAmazonS3 _s3Client;
    private readonly ILogger<S3StorageService> _logger;
    private readonly AwsS3Settings _settings;

    public S3StorageService(
        IAmazonS3 s3Client,
        IOptions<AwsS3Settings> options,
        ILogger<S3StorageService> logger)
    {
        _s3Client = s3Client;
        _settings = options.Value;
        _logger = logger;
    }

    public async Task<string> UploadFileAsync(Stream fileStream, string fileName, string contentType, string folder = "", CancellationToken cancellationToken = default)
    {
        try
        {
            var key = string.IsNullOrEmpty(folder) ? fileName : $"{folder}/{fileName}";

            var uploadRequest = new TransferUtilityUploadRequest
            {
                InputStream = fileStream,
                Key = key,
                ContentType = contentType,
                BucketName = _settings.BucketName,
                CannedACL = S3CannedACL.PublicRead 
            };
            
            if (string.Equals(Path.GetExtension(fileName), ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                uploadRequest.Metadata["Content-Disposition"] = $"attachment; filename=\"{fileName}\"";
            }

            var fileTransferUtility = new TransferUtility(_s3Client);

            await fileTransferUtility.UploadAsync(uploadRequest, cancellationToken);

            return key;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error uploading file to S3");
            throw;
        }
    }

    public async Task DeleteFileAsync(string fileUrl, CancellationToken cancellationToken = default)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(fileUrl))
                return;
            
            if (!Uri.TryCreate(fileUrl, UriKind.Absolute, out var uri))
                return;
            
            var path = uri.AbsolutePath.TrimStart('/');
            
            if (path.StartsWith(_settings.BucketName + "/", StringComparison.OrdinalIgnoreCase))
            {
                path = path.Substring(_settings.BucketName.Length + 1);
            }

            if (string.IsNullOrWhiteSpace(path))
                return;

            await _s3Client.DeleteObjectAsync(
                _settings.BucketName,
                path,
                cancellationToken);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting file from storage");
            throw;
        }
    }
}