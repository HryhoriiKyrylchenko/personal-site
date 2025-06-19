namespace PersonalSite.Web.Models.Analytics;

public class TrackAnalyticsEventRequest
{
    public string EventType { get; set; } = string.Empty;
    public string PageSlug { get; set; } = string.Empty;
    public string AdditionalDataJson { get; set; } = string.Empty;  
}