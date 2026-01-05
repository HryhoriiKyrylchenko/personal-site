using System.Text;
using Npgsql;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Infrastructure.Persistence.Repositories.Common;

public class LogRepository : ILogRepository
{
    private readonly LoggingDbContext _ctx;
    public LogRepository(LoggingDbContext ctx) => _ctx = ctx;
    
    public async Task<PaginatedResult<LogEntry>> GetPaginatedAsync(
        int page,
        int pageSize,
        DateTime? from,
        DateTime? to,
        short level,
        CancellationToken cancellationToken = default)
    {
        var sqlBuilder = new StringBuilder("SELECT * FROM logs WHERE 1 = 1");
        var countSqlBuilder = new StringBuilder("SELECT COUNT(*) FROM logs WHERE 1 = 1");
        var parameters = new List<NpgsqlParameter>();

        if (level > 0)
        {
            sqlBuilder.Append(" AND level = @level");
            countSqlBuilder.Append(" AND level = @level");
            parameters.Add(new NpgsqlParameter("@level", level));
        }

        if (from.HasValue)
        {
            sqlBuilder.Append(" AND timestamp >= @from");
            countSqlBuilder.Append(" AND timestamp >= @from");
            parameters.Add(new NpgsqlParameter("@from", from.Value));
        }

        if (to.HasValue)
        {
            sqlBuilder.Append(" AND timestamp <= @to");
            countSqlBuilder.Append(" AND timestamp <= @to");
            parameters.Add(new NpgsqlParameter("@to", to.Value));
        }

        var totalCount = await _ctx.LogEntries
            .FromSqlRaw(countSqlBuilder.ToString(), parameters.Cast<object>().ToArray())
            .CountAsync(cancellationToken);

        sqlBuilder.Append(" ORDER BY timestamp DESC OFFSET @offset LIMIT @limit");
        parameters.Add(new NpgsqlParameter("@offset", (page - 1) * pageSize));
        parameters.Add(new NpgsqlParameter("@limit", pageSize));

        var logs = await _ctx.LogEntries
            .FromSqlRaw(sqlBuilder.ToString(), parameters.Cast<object>().ToArray())
            .ToListAsync(cancellationToken);

        return PaginatedResult<LogEntry>.Success(logs, page, pageSize, totalCount);
    }
    
    public async Task<int> DeleteOlderThanAsync(DateTime cutoff) =>
        await _ctx.Database.ExecuteSqlRawAsync(
            "DELETE FROM logs WHERE \"timestamp\" < {0}", cutoff);
}