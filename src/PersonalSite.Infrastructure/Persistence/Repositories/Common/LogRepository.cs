using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Infrastructure.Persistence.Repositories.Common;

public class LogRepository : ILogRepository
{
    private readonly LoggingDbContext _ctx;
    public LogRepository(LoggingDbContext ctx) => _ctx = ctx;

    public async Task<List<LogEntry>> GetPaginatedAsync(int count) =>
        await _ctx.LogEntries
            .FromSqlRaw("SELECT * FROM logs ORDER BY \"timestamp\" DESC LIMIT {0}", count)
            .ToListAsync();

    public async Task<int> DeleteOlderThanAsync(DateTime cutoff) =>
        await _ctx.Database.ExecuteSqlRawAsync(
            "DELETE FROM logs WHERE \"timestamp\" < {0}", cutoff);
}