namespace PersonalSite.Infrastructure.Storage;

public interface IS3UrlBuilder
{
    string BuildUrl(string? path);
    string ExtractKey(string url);
}