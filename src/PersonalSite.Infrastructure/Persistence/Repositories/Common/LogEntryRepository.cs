namespace PersonalSite.Infrastructure.Persistence.Repositories.Common;

public class LogEntryRepository : EfRepository<LogEntry>, ILogEntryRepository
{
    public LogEntryRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<LogEntry>> GetByLevelAsync(LogLevel level, CancellationToken cancellationToken = default)
    {
        return await DbContext.Logs
            .Where(log => log.Level == level)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<LogEntry>> GetBySourceAsync(string source, CancellationToken cancellationToken = default)
    {
        return await DbContext.Logs
            .Where(log => log.Source == source)
            .ToListAsync(cancellationToken);
    }
}