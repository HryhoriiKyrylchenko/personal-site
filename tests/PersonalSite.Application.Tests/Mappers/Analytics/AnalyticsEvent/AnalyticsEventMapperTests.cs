using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Mappers;

namespace PersonalSite.Application.Tests.Mappers.Analytics.AnalyticsEvent;

public class AnalyticsEventMapperTests
{
    private readonly AnalyticsEventMapper _mapper = new();

    [Fact]
    public void MapToDto_ShouldMapAllPropertiesCorrectly()
    {
        // Arrange
        var entity = new Domain.Entities.Analytics.AnalyticsEvent
        {
            Id = Guid.NewGuid(),
            EventType = "PageView",
            PageSlug = "/home",
            Referrer = "https://google.com",
            UserAgent = "Mozilla/5.0",
            CreatedAt = DateTime.UtcNow,
            AdditionalDataJson = "{\"key\":\"value\"}"
        };

        // Act
        var dto = _mapper.MapToDto(entity);

        // Assert
        dto.Id.Should().Be(entity.Id);
        dto.EventType.Should().Be(entity.EventType);
        dto.PageSlug.Should().Be(entity.PageSlug);
        dto.Referrer.Should().Be(entity.Referrer);
        dto.UserAgent.Should().Be(entity.UserAgent);
        dto.CreatedAt.Should().Be(entity.CreatedAt);
        dto.AdditionalDataJson.Should().Be(entity.AdditionalDataJson);
    }

    [Fact]
    public void MapToDtoList_ShouldMapAllEntitiesCorrectly()
    {
        // Arrange
        var now = DateTime.UtcNow;
        var entities = new List<Domain.Entities.Analytics.AnalyticsEvent>
        {
            new()
            {
                Id = Guid.NewGuid(),
                EventType = "Click",
                PageSlug = "/about",
                Referrer = null,
                UserAgent = "Chrome",
                CreatedAt = now,
                AdditionalDataJson = null
            },
            new()
            {
                Id = Guid.NewGuid(),
                EventType = "Scroll",
                PageSlug = "/contact",
                Referrer = "https://bing.com",
                UserAgent = "Safari",
                CreatedAt = now.AddSeconds(5),
                AdditionalDataJson = "{\"depth\":90}"
            }
        };

        // Act
        var dtos = _mapper.MapToDtoList(entities);

        // Assert
        dtos.Should().HaveCount(2);

        for (int i = 0; i < entities.Count; i++)
        {
            dtos[i].Id.Should().Be(entities[i].Id);
            dtos[i].EventType.Should().Be(entities[i].EventType);
            dtos[i].PageSlug.Should().Be(entities[i].PageSlug);
            dtos[i].Referrer.Should().Be(entities[i].Referrer);
            dtos[i].UserAgent.Should().Be(entities[i].UserAgent);
            dtos[i].CreatedAt.Should().Be(entities[i].CreatedAt);
            dtos[i].AdditionalDataJson.Should().Be(entities[i].AdditionalDataJson);
        }
    }
}