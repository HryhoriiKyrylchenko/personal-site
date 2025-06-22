using PersonalSite.Application.Features.Common.LogEntries.Dtos;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.LogEntries.Queries.GetLogEntries;

public class GetLogEntriesHandler : IRequestHandler<GetLogEntriesQuery, PaginatedResult<LogEntryDto>>
{
    private readonly ILogEntryRepository _repository;
    private readonly ILogger<GetLogEntriesHandler> _logger;
    private readonly IMapper<LogEntry, LogEntryDto> _mapper;

    public GetLogEntriesHandler(
        ILogEntryRepository repository,
        ILogger<GetLogEntriesHandler> logger,
        IMapper<LogEntry, LogEntryDto> mapper)
    {
        _repository = repository;
        _logger = logger;  
        _mapper = mapper;       
    }

    public async Task<PaginatedResult<LogEntryDto>> Handle(GetLogEntriesQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var query = _repository.GetQueryable().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.LevelFilter))
                query = query.Where(x => x.Level == request.LevelFilter);

            if (!string.IsNullOrWhiteSpace(request.SourceContextFilter))
                query = query.Where(x => x.SourceContext != null && x.SourceContext.Contains(request.SourceContextFilter));

            if (request.From.HasValue)
                query = query.Where(x => x.Timestamp >= request.From.Value);

            if (request.To.HasValue)
                query = query.Where(x => x.Timestamp <= request.To.Value);

            var total = await query.CountAsync(cancellationToken);

            var entities = await query
                .OrderByDescending(x => x.Timestamp)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .AsNoTracking()
                .ToListAsync(cancellationToken);
        
            var items = _mapper.MapToDtoList(entities);

            return PaginatedResult<LogEntryDto>.Success(items, total, request.Page, request.PageSize);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting log entries.");
            return PaginatedResult<LogEntryDto>.Failure("Error getting log entries.");      
        }
    }
}