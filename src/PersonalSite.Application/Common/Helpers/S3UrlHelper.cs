namespace PersonalSite.Application.Common.Helpers;

public static class S3UrlHelper
{
    private const string BucketBaseUrl = "https://your-bucket.s3.amazonaws.com/";

    public static string BuildImageUrl(string? relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
            return string.Empty;

        return $"{BucketBaseUrl.TrimEnd('/')}/{relativePath.TrimStart('/')}";
    }
}
