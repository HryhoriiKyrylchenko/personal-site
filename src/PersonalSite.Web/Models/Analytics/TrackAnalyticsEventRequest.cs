namespace PersonalSite.Web.Models.Analytics;

public class TrackAnalyticsEventRequest
{
    public string EventType { get; set; } = string.Empty;
    public string PageSlug { get; set; } = string.Empty;
    public Dictionary<string, string>? AdditionalData { get; set; }
}