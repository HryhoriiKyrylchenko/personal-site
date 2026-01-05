using System.Text.Json;

namespace PersonalSite.Domain.Entities.Common;

public class LogEntry
{
    public DateTime Timestamp { get; set; }
    public int Level { get; set; }
    public string? Message { get; set; } = string.Empty;
    public string? MessageTemplate { get; set; } = string.Empty;
    public string? Exception { get; set; } = string.Empty;
    public JsonDocument Properties { get; set; } = JsonDocument.Parse("{}");
    public string? SourceContext { get; set; } = string.Empty;
}