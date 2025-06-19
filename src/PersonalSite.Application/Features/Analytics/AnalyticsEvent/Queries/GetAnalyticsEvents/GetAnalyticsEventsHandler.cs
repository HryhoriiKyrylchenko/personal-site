namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Queries.GetAnalyticsEvents;

public class GetAnalyticsEventsHandler : IRequestHandler<GetAnalyticsEventsQuery, PaginatedResult<AnalyticsEventDto>>
{
    private readonly IAnalyticsEventRepository _repository;
    private readonly ILogger<GetAnalyticsEventsHandler> _logger;

    public GetAnalyticsEventsHandler(
        IAnalyticsEventRepository repository,
        ILogger<GetAnalyticsEventsHandler> logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public async Task<PaginatedResult<AnalyticsEventDto>> Handle(GetAnalyticsEventsQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var page = Math.Max(request.Page, 1);
            var pageSize = Math.Clamp(request.PageSize, 1, 100);

            var query = _repository.GetQueryable().AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.EventType))
                query = query.Where(x => x.EventType == request.EventType);

            if (!string.IsNullOrWhiteSpace(request.PageSlug))
                query = query.Where(x => x.PageSlug == request.PageSlug);

            if (request.From.HasValue)
                query = query.Where(x => x.CreatedAt >= request.From.Value);

            if (request.To.HasValue)
                query = query.Where(x => x.CreatedAt <= request.To.Value);

            var total = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(x => EntityToDtoMapper.MapAnalyticsEventToDto(x))
                .ToListAsync(cancellationToken);

            return PaginatedResult<AnalyticsEventDto>.Success(items, total, request.Page, request.PageSize);

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting analytics events.");
            return PaginatedResult<AnalyticsEventDto>.Failure("An error occurred while getting the analytics events.");       
        }
    }
}