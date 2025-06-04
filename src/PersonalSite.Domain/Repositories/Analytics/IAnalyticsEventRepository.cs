namespace PersonalSite.Domain.Repositories.Analytics;

public interface IAnalyticsEventRepository : IRepository<AnalyticsEvent>
{
    Task<List<AnalyticsEvent>> GetRecentAsync(int count, CancellationToken cancellationToken = default);
    Task<int> CountByEventTypeAsync(string eventType, CancellationToken cancellationToken = default);
    Task<List<AnalyticsEvent>> GetByPageSlugAsync(string slug, CancellationToken cancellationToken = default);
}