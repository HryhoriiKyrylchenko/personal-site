using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Domain.Interfaces.Repositories.Common;

public interface ILogRepository
{
    Task<List<LogEntry>> GetPaginatedAsync(int count);
    Task<int> DeleteOlderThanAsync(DateTime cutoff);
}