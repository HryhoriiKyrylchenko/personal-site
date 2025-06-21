namespace PersonalSite.Infrastructure.Persistence.Repositories.Common;

public class LogEntryRepository : EfRepository<LogEntry>, ILogEntryRepository
{
    public LogEntryRepository(
        ApplicationDbContext context, 
        ILogger<LogEntryRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<List<LogEntry>> GetByIdsAsync(List<Guid> requestIds, CancellationToken cancellationToken)
    {
        return await DbContext.Logs
            .Where(l => requestIds.Contains(l.Id))
            .ToListAsync(cancellationToken); 
    }
}