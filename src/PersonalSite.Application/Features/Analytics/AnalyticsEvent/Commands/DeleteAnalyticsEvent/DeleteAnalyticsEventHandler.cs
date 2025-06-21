namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.DeleteAnalyticsEvent;

public class DeleteAnalyticsEventHandler : IRequestHandler<DeleteAnalyticsEventCommand, Result>
{
    private readonly IAnalyticsEventRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<DeleteAnalyticsEventHandler> _logger;

    public DeleteAnalyticsEventHandler(
        IAnalyticsEventRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<DeleteAnalyticsEventHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(DeleteAnalyticsEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity is null)
                return Result.Failure("AnalyticsEvent not found.");

            _repository.Remove(entity);
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