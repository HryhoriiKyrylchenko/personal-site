namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Queries.GetAnalyticsEvents;

public record GetAnalyticsEventsQuery(
    int Page = 1,
    int PageSize = 20,
    string? EventType = null,
    string? PageSlug = null,
    DateTime? From = null,
    DateTime? To = null
) : IRequest<PaginatedResult<AnalyticsEventDto>>;
