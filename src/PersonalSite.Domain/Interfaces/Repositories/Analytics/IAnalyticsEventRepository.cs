using PersonalSite.Domain.Entities.Analytics;

namespace PersonalSite.Domain.Interfaces.Repositories.Analytics;

public interface IAnalyticsEventRepository : IRepository<AnalyticsEvent>
{
    Task<PaginatedResult<AnalyticsEvent>> GetFilteredAsync(
        int page,
        int pageSize,
        string? eventType,
        string? pageSlug,
        DateTime? from,
        DateTime? to,
        CancellationToken cancellationToken = default);
}