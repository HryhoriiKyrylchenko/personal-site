namespace PersonalSite.Domain.Interfaces.Repositories.Common;

public interface ILogEntryRepository : IRepository<LogEntry>
{
    Task<List<LogEntry>> GetByLevelAsync(string level, CancellationToken cancellationToken = default);
}