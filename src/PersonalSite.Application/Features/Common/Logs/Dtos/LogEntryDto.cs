namespace PersonalSite.Application.Features.Common.Logs.Dtos;

public record LogEntryDto(
    DateTime Timestamp,
    int Level,
    string Message,
    string MessageTemplate,
    string Exception,
    JsonDocument Properties,
    string SourceContext
);