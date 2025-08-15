using System.Text.Json.Serialization;

namespace PersonalSite.Web.Models.Analytics;

public class TrackAnalyticsEventRequest
{
    [JsonPropertyName("eventType")]
    public string EventType { get; set; } = string.Empty;
    [JsonPropertyName("pageSlug")]
    public string PageSlug { get; set; } = string.Empty;
    
    [JsonPropertyName("additionalDataJson")]
    public string AdditionalDataJson { get; set; } = string.Empty;  
}