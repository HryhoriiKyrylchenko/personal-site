using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Dtos;
using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Queries.GetAnalyticsEvents;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Interfaces.Repositories.Analytics;

namespace PersonalSite.Application.Tests.Handlers.Analytics.AnalyticsEvent;

public class GetAnalyticsEventsQueryHandlerTests
{
    private readonly Mock<IAnalyticsEventRepository> _repositoryMock = new();
    private readonly Mock<IMapper<Domain.Entities.Analytics.AnalyticsEvent, AnalyticsEventDto>> _mapperMock = new();
    private readonly Mock<ILogger<GetAnalyticsEventsQueryHandler>> _loggerMock = new();

    private readonly GetAnalyticsEventsQueryHandler _handler;

    public GetAnalyticsEventsQueryHandlerTests()
    {
        _handler = new GetAnalyticsEventsQueryHandler(
            _repositoryMock.Object,
            _mapperMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldReturnPaginatedResult_WhenDataExists()
    {
        // Arrange
        var events = new List<Domain.Entities.Analytics.AnalyticsEvent>
        {
            AnalyticsTestDataFactory.CreateAnalyticsEvent(),
            AnalyticsTestDataFactory.CreateAnalyticsEvent()
        };

        var dtos = events.Select(AnalyticsTestDataFactory.MapToDto).ToList();

        var pagedResult = PaginatedResult<Domain.Entities.Analytics.AnalyticsEvent>.Success(events, 1, 20, events.Count);

        _repositoryMock.Setup(r => r.GetFilteredAsync(
            1, 20, null, null, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(pagedResult);

        _mapperMock.Setup(m => m.MapToDtoList(events)).Returns(dtos);

        var query = new GetAnalyticsEventsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(dtos);
        result.TotalCount.Should().Be(2);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenRepositoryFails()
    {
        // Arrange
        var failedResult = PaginatedResult<Domain.Entities.Analytics.AnalyticsEvent>.Failure("No data");

        _repositoryMock.Setup(r => r.GetFilteredAsync(
            1, 20, null, null, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(failedResult);

        var query = new GetAnalyticsEventsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Analytics events not found");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetFilteredAsync(
            It.IsAny<int>(), It.IsAny<int>(), It.IsAny<string?>(), It.IsAny<string?>(), 
            It.IsAny<DateTime?>(), It.IsAny<DateTime?>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("boom"));

        var query = new GetAnalyticsEventsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An error occurred while getting the analytics events.");
    }
}