namespace PersonalSite.Application.Services.Analytics.Requests;

public class AnalyticsEventUpdateRequest
{
    public Guid Id { get; set; }
    public string PageSlug { get; set; } = string.Empty;
    public string? Referrer { get; set; }
    public string? UserAgent { get; set; }
    public string? AdditionalDataJson { get; set; }  
}