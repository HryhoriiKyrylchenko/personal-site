namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Events.PageViewedEvent;

public record PageViewedEvent(
    string PageSlug,
    string Referrer,
    string UserAgent
) : INotification;