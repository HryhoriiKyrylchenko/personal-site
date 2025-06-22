using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Domain.Interfaces.Repositories.Common;

public interface ILogEntryRepository : IRepository<LogEntry>
{
    Task<List<LogEntry>> GetByIdsAsync(List<Guid> requestIds, CancellationToken cancellationToken);
}