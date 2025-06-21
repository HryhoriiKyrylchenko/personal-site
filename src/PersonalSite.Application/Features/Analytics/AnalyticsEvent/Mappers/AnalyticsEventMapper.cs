namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Mappers;

public static class AnalyticsEventMapper
{
    public static AnalyticsEventDto MapToDto(Domain.Entities.Analytics.AnalyticsEvent entity)
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
    
    public static List<AnalyticsEventDto> MapToDtoList(IEnumerable<Domain.Entities.Analytics.AnalyticsEvent> entities)
    {
        return entities.Select(MapToDto).ToList();
    }
}