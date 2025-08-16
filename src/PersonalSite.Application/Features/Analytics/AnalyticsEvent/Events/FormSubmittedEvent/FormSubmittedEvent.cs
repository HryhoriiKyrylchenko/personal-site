namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Events.FormSubmittedEvent;

public record FormSubmittedEvent(
    string PageSlug,
    string Referrer,
    string UserAgent,
    string AdditionalDataJson
) : INotification;