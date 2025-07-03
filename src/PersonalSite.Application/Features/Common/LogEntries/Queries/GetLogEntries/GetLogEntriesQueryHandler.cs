using PersonalSite.Application.Features.Common.LogEntries.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.LogEntries.Queries.GetLogEntries;

public class GetLogEntriesQueryHandler : IRequestHandler<GetLogEntriesQuery, PaginatedResult<LogEntryDto>>
{
    private readonly ILogEntryRepository _repository;
    private readonly ILogger<GetLogEntriesQueryHandler> _logger;
    private readonly IMapper<LogEntry, LogEntryDto> _mapper;

    public GetLogEntriesQueryHandler(
        ILogEntryRepository repository,
        ILogger<GetLogEntriesQueryHandler> logger,
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
            var logs = await _repository.GetFilteredAsync(
                request.Page,
                request.PageSize,
                request.LevelFilter,
                request.SourceContextFilter,
                request.From,
                request.To,
                cancellationToken);

            if (logs.IsFailure || logs.Value == null)
            {
                _logger.LogWarning("Log entries not found");
                return PaginatedResult<LogEntryDto>.Failure("Log entries not found");
            }
        
            var items = _mapper.MapToDtoList(logs.Value);

            return PaginatedResult<LogEntryDto>.Success(items, logs.PageNumber, logs.PageSize, logs.TotalCount);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error getting log entries.");
            return PaginatedResult<LogEntryDto>.Failure("Error getting log entries.");      
        }
    }
}