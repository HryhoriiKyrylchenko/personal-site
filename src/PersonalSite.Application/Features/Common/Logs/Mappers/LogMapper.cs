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
            Message: entity.Message ?? string.Empty,
            MessageTemplate:entity.MessageTemplate?? string.Empty,
            Exception: entity.Exception?? string.Empty,
            Properties: entity.Properties,
            SourceContext: entity.SourceContext?? string.Empty
        );
    }

    public List<LogEntryDto> MapToDtoList(IEnumerable<LogEntry> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}