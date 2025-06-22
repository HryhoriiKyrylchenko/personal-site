namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Dtos;

public class AnalyticsEventDto
{
    public Guid Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string PageSlug { get; set; } = string.Empty;
    public string? Referrer { get; set; }
    public string? UserAgent { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? AdditionalDataJson { get; set; }  
}