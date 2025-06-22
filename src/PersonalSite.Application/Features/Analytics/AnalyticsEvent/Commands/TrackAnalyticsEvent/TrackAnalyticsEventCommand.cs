using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.TrackAnalyticsEvent;

public record TrackAnalyticsEventCommand(
    string EventType,
    string PageSlug,
    string? Referrer,
    string? UserAgent,
    string? AdditionalDataJson
) : IRequest<Result>;