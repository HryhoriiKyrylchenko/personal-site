namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.TrackAnalyticsEvent;

public class TrackAnalyticsEventHandler : IRequestHandler<TrackAnalyticsEventCommand, Result>
{
    private readonly IAnalyticsEventRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public TrackAnalyticsEventHandler(IAnalyticsEventRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<Result> Handle(TrackAnalyticsEventCommand request, CancellationToken cancellationToken)
    {
        var analyticsEvent = new Domain.Entities.Analytics.AnalyticsEvent
        {
            Id = Guid.NewGuid(),
            EventType = request.EventType,
            PageSlug = request.PageSlug,
            Referrer = request.Referrer,
            UserAgent = request.UserAgent,
            CreatedAt = DateTime.UtcNow,
            AdditionalDataJson = request.AdditionalData != null 
                ? JsonSerializer.Serialize(request.AdditionalData) 
                : null
        };

        await _repository.AddAsync(analyticsEvent, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
