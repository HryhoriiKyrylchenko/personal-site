namespace PersonalSite.Application.Services.Common.DTOs;

public class LogEntryDto
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public string Level { get; set; } = string.Empty;
    public string Source { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Exception { get; set; } 
}