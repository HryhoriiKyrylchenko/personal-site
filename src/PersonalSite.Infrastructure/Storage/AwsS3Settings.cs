namespace PersonalSite.Infrastructure.Storage;

public class AwsS3Settings
{
    public string BucketName { get; set; } = string.Empty;
    public string Region { get; set; } = string.Empty;
    public string PublicBaseUrl { get; set; } = string.Empty;
}