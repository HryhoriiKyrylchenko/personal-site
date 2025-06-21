namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.TrackAnalyticsEvent;

public class TrackAnalyticsEventHandler : IRequestHandler<TrackAnalyticsEventCommand, Result>
{
    private readonly IAnalyticsEventRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<TrackAnalyticsEventHandler> _logger;

    public TrackAnalyticsEventHandler(
        IAnalyticsEventRepository repository, 
        IUnitOfWork unitOfWork,
        ILogger<TrackAnalyticsEventHandler> logger)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<Result> Handle(TrackAnalyticsEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            var analyticsEvent = new Domain.Entities.Analytics.AnalyticsEvent
            {
                Id = Guid.NewGuid(),
                EventType = request.EventType,
                PageSlug = request.PageSlug,
                Referrer = request.Referrer,
                UserAgent = request.UserAgent,
                CreatedAt = DateTime.UtcNow,
                AdditionalDataJson = request.AdditionalDataJson
            };

            await _repository.AddAsync(analyticsEvent, cancellationToken);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while tracking analytics event.");
            return Result.Failure("An error occurred while tracking the event.");
        }
    }
}