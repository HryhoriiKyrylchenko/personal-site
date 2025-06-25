using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.TrackAnalyticsEvent;
using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Dtos;
using PersonalSite.Domain.Entities.Analytics;

namespace PersonalSite.Application.Tests.Fixtures.TestDataFactories;

public static class AnalyticsTestDataFactory
{
    public static AnalyticsEvent CreateAnalyticsEvent(
        Guid? id = null,
        string eventType = "PageView",
        string pageSlug = "home",
        string? referrer = null,
        string? userAgent = null,
        DateTime? createdAt = null,
        string? additionalDataJson = null)
    {
        return new AnalyticsEvent
        {
            Id = id ?? Guid.NewGuid(),
            EventType = eventType,
            PageSlug = pageSlug,
            Referrer = referrer,
            UserAgent = userAgent,
            CreatedAt = createdAt ?? DateTime.UtcNow,
            AdditionalDataJson = additionalDataJson
        };
    }

    public static AnalyticsEventDto MapToDto(AnalyticsEvent e) => new()
    {
        Id = e.Id,
        EventType = e.EventType,
        PageSlug = e.PageSlug,
        Referrer = e.Referrer,
        UserAgent = e.UserAgent,
        CreatedAt = e.CreatedAt,
        AdditionalDataJson = e.AdditionalDataJson
    };
    
    public static TrackAnalyticsEventCommand CreateTrackAnalyticsEventCommand(
        string eventType = "PageView",
        string pageSlug = "/home",
        string? referrer = "https://google.com",
        string? userAgent = "Mozilla/5.0",
        string? additionalDataJson = "{\"source\":\"ad\"}")
    {
        return new TrackAnalyticsEventCommand(
            EventType: eventType,
            PageSlug: pageSlug,
            Referrer: referrer,
            UserAgent: userAgent,
            AdditionalDataJson: additionalDataJson
        );
    }
}