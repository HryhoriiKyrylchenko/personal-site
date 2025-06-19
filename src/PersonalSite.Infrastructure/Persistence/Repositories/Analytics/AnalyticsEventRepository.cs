namespace PersonalSite.Infrastructure.Persistence.Repositories.Analytics;

public class AnalyticsEventRepository : EfRepository<AnalyticsEvent>, IAnalyticsEventRepository
{
    public AnalyticsEventRepository(
        ApplicationDbContext context, 
        ILogger<AnalyticsEventRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public IQueryable<AnalyticsEvent> GetQueryable()
    {
        return DbContext.AnalyticsEvents.AsNoTracking();
    }
}