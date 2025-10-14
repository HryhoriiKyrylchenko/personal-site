using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Domain.Interfaces.Repositories.Common;

public interface ILogRepository
{
    public Task<PaginatedResult<LogEntry>> GetPaginatedAsync(
        int page,
        int pageSize,
        DateTime? from,
        DateTime? to,
        short level,
        CancellationToken cancellationToken = default);
    Task<int> DeleteOlderThanAsync(DateTime cutoff);
}