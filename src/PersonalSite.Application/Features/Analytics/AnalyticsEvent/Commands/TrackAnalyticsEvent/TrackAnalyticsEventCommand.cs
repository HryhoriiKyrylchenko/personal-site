namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.TrackAnalyticsEvent;

public record TrackAnalyticsEventCommand(
    string EventType,
    string PageSlug,
    string? Referrer,
    string? UserAgent,
    string? IpAddress,
    Dictionary<string, string>? AdditionalData
) : IRequest<Result>;