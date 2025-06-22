using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Domain.Interfaces.Repositories.Common;

public interface ILogEntryRepository : IRepository<LogEntry>
{
    Task<List<LogEntry>> GetByIdsAsync(List<Guid> requestIds, CancellationToken cancellationToken);
    Task<PaginatedResult<LogEntry>> GetFilteredAsync(
        int page,
        int pageSize,
        string? levelFilter,
        string? sourceContextFilter,
        DateTime? from,
        DateTime? to,
        CancellationToken cancellationToken = default);
}