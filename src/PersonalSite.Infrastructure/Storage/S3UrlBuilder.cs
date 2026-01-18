namespace PersonalSite.Infrastructure.Storage;

public class S3UrlBuilder : IS3UrlBuilder
{
    private readonly string _baseUrl;

    public S3UrlBuilder(IOptions<AwsS3Settings> options)
    {
        _baseUrl = options.Value.PublicBaseUrl.TrimEnd('/') + "/";
    }

    public string BuildUrl(string? path)
    {
        return string.IsNullOrWhiteSpace(path)
            ? string.Empty
            : $"{_baseUrl}{path.TrimStart('/')}";
    }

    public string ExtractKey(string url)
    {
        if (string.IsNullOrWhiteSpace(url))
            return string.Empty;

        if (!url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
            return url.TrimStart('/');

        if (!url.StartsWith(_baseUrl, StringComparison.OrdinalIgnoreCase))
            throw new InvalidOperationException($"URL does not belong to this storage: {url}");

        return url
            .Substring(_baseUrl.Length)
            .TrimStart('/');
    }
}