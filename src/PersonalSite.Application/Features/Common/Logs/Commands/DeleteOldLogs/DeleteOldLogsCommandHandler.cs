using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.Logs.Commands.DeleteOldLogs;

public class DeleteOldLogsHandler : IRequestHandler<DeleteOldLogsCommand, Result<int>>
{
    private readonly ILogRepository _repository;
    private readonly ILogger<DeleteOldLogsHandler> _logger;

    public DeleteOldLogsHandler(
        ILogRepository repository,
        ILogger<DeleteOldLogsHandler> logger
    )
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(DeleteOldLogsCommand request, CancellationToken ct)
    {
        try
        {
            var result = await _repository.DeleteOlderThanAsync(request.Cutoff);
            return Result<int>.Success(result);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while deleting logs");
            return Result<int>.Failure("Error occurred while deleting logs");      
        }
    }
}