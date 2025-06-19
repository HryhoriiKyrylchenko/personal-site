namespace PersonalSite.Application.Services.Common.Requests;

public class LogEntryAddRequest
{
    public string Level { get; set; } = string.Empty;
    public string Message { get; set; } = string.Empty;
    public string MessageTemplate { get; set; } = string.Empty;
    public string? Exception { get; set; }
    public string? Properties { get; set; }
    public string? SourceContext { get; set; } 
}