namespace PersonalSite.Application.Features.Common.LogEntries.Mappers;

public static class LogEntryMapper
{
    public static LogEntryDto MapToDto(LogEntry entity)
    {
        return new LogEntryDto
        {
            Id = entity.Id,
            Timestamp = entity.Timestamp,
            Level = entity.Level,
            Message = entity.Message,
            MessageTemplate = entity.MessageTemplate,
            Exception = entity.Exception,
            Properties = entity.Properties,
            SourceContext = entity.SourceContext
        };
    }
    
    public static List<LogEntryDto> MapToDtoList(IEnumerable<LogEntry> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}