using PersonalSite.Application.Features.Common.Logs.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.Logs.Queries.GetRecentLogs;

public class GetRecentLogsHandler : IRequestHandler<GetRecentLogsQuery, Result<List<LogEntryDto>>>
{
    private readonly ILogRepository _repository;
    private readonly ILogger<GetRecentLogsHandler> _logger;   
    private readonly IMapper<LogEntry, LogEntryDto> _mapper;   
    
    public GetRecentLogsHandler(
        ILogRepository repository,
        ILogger<GetRecentLogsHandler> logger,
        IMapper<LogEntry, LogEntryDto> mapper
        )
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;       
    }

    public async Task<Result<List<LogEntryDto>>> Handle(GetRecentLogsQuery request, CancellationToken ct)
    {
        try
        {
            var logs = await _repository.GetPaginatedAsync(request.Count);
            var result = _mapper.MapToDtoList(logs);
            return Result<List<LogEntryDto>>.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while retrieving logs.");
            return Result<List<LogEntryDto>>.Failure("An unexpected error occurred.");      
        }
    }
}