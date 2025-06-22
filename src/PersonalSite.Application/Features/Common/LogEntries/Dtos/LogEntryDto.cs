namespace PersonalSite.Application.Features.Common.LogEntries.Dtos;

public class LogEntryDto
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string MessageTemplate { get; set; } = string.Empty;
    public string? Exception { get; set; } 
    public string? Properties { get; set; }
    public string? SourceContext { get; set; } 
}