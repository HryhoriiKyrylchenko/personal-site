namespace PersonalSite.Infrastructure.Persistence.Repositories.Common;

public class LogEntryRepository : EfRepository<LogEntry>, ILogEntryRepository
{
    public LogEntryRepository(
        ApplicationDbContext context, 
        ILogger<LogEntryRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<List<LogEntry>> GetByLevelAsync(string level, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(level))
            throw new ArgumentException("Level cannot be null or whitespace", nameof(level));
        
        return await DbContext.Logs
            .Where(log => log.Level == level)
            .ToListAsync(cancellationToken);
    }
}