using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Analytics;
using PersonalSite.Domain.Interfaces.Repositories.Analytics;

namespace PersonalSite.Infrastructure.Persistence.Repositories.Analytics;

public class AnalyticsEventRepository : EfRepository<AnalyticsEvent>, IAnalyticsEventRepository
{
    public AnalyticsEventRepository(
        ApplicationDbContext context, 
        ILogger<AnalyticsEventRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<PaginatedResult<AnalyticsEvent>> GetFilteredAsync(int page, int pageSize, string? eventType, string? pageSlug, DateTime? from, DateTime? to,
        CancellationToken cancellationToken = default)
    {
        var query = DbContext.AnalyticsEvents.AsQueryable().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(eventType))
            query = query.Where(x => x.EventType == eventType);

        if (!string.IsNullOrWhiteSpace(pageSlug))
            query = query.Where(x => x.PageSlug == pageSlug);

        if (from.HasValue)
            query = query.Where(x => x.CreatedAt >= from.Value);

        if (to.HasValue)
            query = query.Where(x => x.CreatedAt <= to.Value);

        var total = await query.CountAsync(cancellationToken);
        var entities = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
        
        return PaginatedResult<AnalyticsEvent>.Success(entities, page, pageSize, total);   
    }
}