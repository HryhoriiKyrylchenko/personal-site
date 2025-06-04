namespace PersonalSite.Domain.Repositories.Common;

public interface ILogEntryRepository : IRepository<LogEntry>
{
    Task<List<LogEntry>> GetByLevelAsync(LogLevel level, CancellationToken cancellationToken = default);
    Task<List<LogEntry>> GetBySourceAsync(string source, CancellationToken cancellationToken = default);
}