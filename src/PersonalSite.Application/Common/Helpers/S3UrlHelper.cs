namespace PersonalSite.Application.Common.Helpers;

public static class S3UrlHelper
{
    private const string BucketBaseUrl = "https://bucket.s3.amazonaws.com/";

    public static string BuildImageUrl(string? relativePath)
    {
        return string.IsNullOrWhiteSpace(relativePath) 
            ? string.Empty 
            : $"{BucketBaseUrl.TrimEnd('/')}/{relativePath.TrimStart('/')}";
    }
}
