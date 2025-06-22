namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Mappers;

public class AnalyticsEventMapper : IMapper<Domain.Entities.Analytics.AnalyticsEvent, AnalyticsEventDto>
{
    public AnalyticsEventDto MapToDto(Domain.Entities.Analytics.AnalyticsEvent entity)
    {
        return new AnalyticsEventDto
        {
            Id = entity.Id,
            EventType = entity.EventType,
            PageSlug = entity.PageSlug,
            Referrer = entity.Referrer,
            UserAgent = entity.UserAgent,
            CreatedAt = entity.CreatedAt,
            AdditionalDataJson = entity.AdditionalDataJson
        };
    }
    
    public List<AnalyticsEventDto> MapToDtoList(IEnumerable<Domain.Entities.Analytics.AnalyticsEvent> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}