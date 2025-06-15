namespace PersonalSite.Infrastructure.Persistence.Repositories.Analytics;

public class AnalyticsEventRepository : EfRepository<AnalyticsEvent>, IAnalyticsEventRepository
{
    public AnalyticsEventRepository(
        ApplicationDbContext context, 
        ILogger<AnalyticsEventRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<List<AnalyticsEvent>> GetRecentAsync(int count, CancellationToken cancellationToken = default)
    {
        if (count <= 0)
            return [];
        
        return await DbContext.AnalyticsEvents
            .OrderByDescending(e => e.CreatedAt)
            .Take(count)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountByEventTypeAsync(string eventType, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(eventType) || eventType.Length > 255)
            return 0;
    
        return await DbContext.AnalyticsEvents
            .CountAsync(e => e.EventType == eventType, cancellationToken);
    }

    public async Task<List<AnalyticsEvent>> GetByPageSlugAsync(string slug, CancellationToken cancellationToken = default)
    {
        return await DbContext.AnalyticsEvents
            .Where(e => e.PageSlug == slug)
            .OrderByDescending(e => e.CreatedAt)
            .ToListAsync(cancellationToken);
    }
}