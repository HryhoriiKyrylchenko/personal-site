using PersonalSite.Application.Features.Common.LogEntries.Dtos;
using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Application.Features.Common.LogEntries.Mappers;

public class LogEntryMapper : IMapper<LogEntry, LogEntryDto>
{
    public LogEntryDto MapToDto(LogEntry entity)
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
    
    public List<LogEntryDto> MapToDtoList(IEnumerable<LogEntry> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}