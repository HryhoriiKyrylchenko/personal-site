using PersonalSite.Application.Features.Common.Language.Dtos;
using PersonalSite.Application.Features.Common.Logs.Dtos;
using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Application.Features.Common.Logs.Mappers;

public class LogMapper: IMapper<LogEntry, LogEntryDto>
{
    public LogEntryDto MapToDto(LogEntry entity)
    {
        return new LogEntryDto
        (
            Timestamp: entity.Timestamp,
            Level: entity.Level,
            Message: entity.Message,
            MessageTemplate:entity.MessageTemplate,
            Exception: entity.Exception,
            Properties: entity.Properties,
            SourceContext: entity.SourceContext
        );
    }

    public List<LogEntryDto> MapToDtoList(IEnumerable<LogEntry> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}