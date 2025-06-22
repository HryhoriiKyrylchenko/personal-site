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
}