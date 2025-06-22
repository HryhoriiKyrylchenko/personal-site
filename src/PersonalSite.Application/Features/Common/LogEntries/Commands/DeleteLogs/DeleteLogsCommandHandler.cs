using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Features.Common.LogEntries.Commands.DeleteLogs;

public class DeleteLogsCommandHandler : IRequestHandler<DeleteLogsCommand, Result>
{
    private readonly ILogEntryRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteLogsCommandHandler> _logger;

    public DeleteLogsCommandHandler(ILogEntryRepository repository, IUnitOfWork unitOfWork, ILogger<DeleteLogsCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteLogsCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var logs = await _repository.GetByIdsAsync(request.Ids, cancellationToken);

            if (logs.Count != request.Ids.Count)
                return Result.Failure("Some log entries were not found.");

            foreach (var log in logs)
                _repository.Remove(log);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to delete log entries.");
            return Result.Failure("Failed to delete log entries.");
        }
    }
}
