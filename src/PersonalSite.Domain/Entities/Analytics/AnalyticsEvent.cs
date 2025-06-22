namespace PersonalSite.Domain.Entities.Analytics;

[Table("AnalyticsEvents")]
public class AnalyticsEvent
{
    [Key]
    public Guid Id { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string PageSlug { get; set; } = string.Empty;
    public string? Referrer { get; set; }
    public string? UserAgent { get; set; }
    public DateTime CreatedAt { get; set; }
    [Column(TypeName = "jsonb")]
    public string? AdditionalDataJson { get; set; }  
}