namespace PersonalSite.Infrastructure.Storage.S3ReferenceProviders;

public static class AsyncEnumerableExtensions
{
    public static async Task<List<T>> SelectManyAsync<T>(
        this IEnumerable<IS3ReferenceProvider> providers,
        Func<IS3ReferenceProvider, Task<IEnumerable<T>>> selector)
    {
        var results = await Task.WhenAll(providers.Select(selector));
        return results.SelectMany(r => r).ToList();
    }
}