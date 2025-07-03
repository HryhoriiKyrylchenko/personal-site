using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Dtos;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Analytics;

namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Queries.GetAnalyticsEvents;

public class GetAnalyticsEventsQueryHandler : IRequestHandler<GetAnalyticsEventsQuery, PaginatedResult<AnalyticsEventDto>>
{
    private readonly IAnalyticsEventRepository _repository;
    private readonly ILogger<GetAnalyticsEventsQueryHandler> _logger;
    private readonly IMapper<Domain.Entities.Analytics.AnalyticsEvent, AnalyticsEventDto> _mapper;

    public GetAnalyticsEventsQueryHandler(
        IAnalyticsEventRepository repository,
        IMapper<Domain.Entities.Analytics.AnalyticsEvent, AnalyticsEventDto> mapper,
        ILogger<GetAnalyticsEventsQueryHandler> logger)
    {
        _repository = repository;
        _mapper = mapper;       
        _logger = logger;
    }

    public async Task<PaginatedResult<AnalyticsEventDto>> Handle(GetAnalyticsEventsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var events = await _repository.GetFilteredAsync(
                request.Page, 
                request.PageSize,
                request.EventType,
                request.PageSlug,
                request.From,
                request.To,
                cancellationToken);

            if (events.IsFailure || events.Value == null)
            {
                _logger.LogWarning("Analytics events not found");
                return PaginatedResult<AnalyticsEventDto>.Failure("Analytics events not found");
            }
            
            var items = _mapper.MapToDtoList(events.Value);

            return PaginatedResult<AnalyticsEventDto>.Success(items, events.PageNumber, events.PageSize, events.TotalCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting analytics events.");
            return PaginatedResult<AnalyticsEventDto>.Failure("An error occurred while getting the analytics events.");       
        }
    }
}