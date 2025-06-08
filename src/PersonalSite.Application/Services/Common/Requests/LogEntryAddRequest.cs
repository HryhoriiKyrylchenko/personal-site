namespace PersonalSite.Application.Services.Common.Requests;

public class LogEntryAddRequest
{
    public LogLevel Level { get; set; }
    public string Source { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string? Exception { get; set; }
}