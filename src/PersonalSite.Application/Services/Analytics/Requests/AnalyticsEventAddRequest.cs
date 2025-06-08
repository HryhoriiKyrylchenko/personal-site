namespace PersonalSite.Application.Services.Analytics.Requests;

public class AnalyticsEventAddRequest
{
    public string EventType { get; set; } = string.Empty;
    public string PageSlug { get; set; } = string.Empty;
    public string? Referrer { get; set; }
    public string? UserAgent { get; set; }
    public string? AdditionalDataJson { get; set; }
}