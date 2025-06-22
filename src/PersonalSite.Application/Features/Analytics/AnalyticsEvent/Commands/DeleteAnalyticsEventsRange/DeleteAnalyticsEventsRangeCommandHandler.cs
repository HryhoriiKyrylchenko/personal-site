using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Analytics;

namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.DeleteAnalyticsEventsRange;

public class DeleteAnalyticsEventsRangeCommandHandler : IRequestHandler<DeleteAnalyticsEventsRangeCommand, Result>
{
    private readonly IAnalyticsEventRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteAnalyticsEventsRangeCommandHandler> _logger;

    public DeleteAnalyticsEventsRangeCommandHandler(
        IAnalyticsEventRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<DeleteAnalyticsEventsRangeCommandHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<Result> Handle(DeleteAnalyticsEventsRangeCommand request, CancellationToken cancellationToken)
    {
        try
        {
            foreach (var id in request.Ids)
            {
                var entity = await _repository.GetByIdAsync(id, cancellationToken);
                if (entity is null)
                {
                    _logger.LogWarning($"AnalyticsEvent with id {id} not found.");
                    continue;   
                }

                _repository.Remove(entity);
            }
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred while deleting analytics event.");
            return Result.Failure("An error occurred while deleting the event.");       
        }
    }
}