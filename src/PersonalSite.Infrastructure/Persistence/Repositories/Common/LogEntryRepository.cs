using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Infrastructure.Persistence.Repositories.Common;

public class LogEntryRepository : EfRepository<LogEntry>, ILogEntryRepository
{
    public LogEntryRepository(
        ApplicationDbContext context, 
        ILogger<LogEntryRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<List<LogEntry>> GetByIdsAsync(List<Guid> requestIds, CancellationToken cancellationToken)
    {
        return await DbContext.Logs
            .Where(l => requestIds.Contains(l.Id))
            .ToListAsync(cancellationToken); 
    }

    public async Task<PaginatedResult<LogEntry>> GetFilteredAsync(int page, int pageSize, string? levelFilter, string? sourceContextFilter, DateTime? from,
        DateTime? to, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Logs.AsQueryable().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(levelFilter))
            query = query.Where(x => x.Level == levelFilter);

        if (!string.IsNullOrWhiteSpace(sourceContextFilter))
            query = query.Where(x => x.SourceContext != null && x.SourceContext.Contains(sourceContextFilter));

        if (from.HasValue)
            query = query.Where(x => x.Timestamp >= from.Value);

        if (to.HasValue)
            query = query.Where(x => x.Timestamp <= to.Value);

        var total = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderByDescending(x => x.Timestamp)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return PaginatedResult<LogEntry>.Success(entities, page, pageSize, total);   
    }
}