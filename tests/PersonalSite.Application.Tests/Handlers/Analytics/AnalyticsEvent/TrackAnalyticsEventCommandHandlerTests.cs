using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.TrackAnalyticsEvent;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Analytics;

namespace PersonalSite.Application.Tests.Handlers.Analytics.AnalyticsEvent;

public class TrackAnalyticsEventCommandHandlerTests
{
    private readonly Mock<IAnalyticsEventRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<TrackAnalyticsEventCommandHandler>> _loggerMock = new();

    private readonly TrackAnalyticsEventCommandHandler _handler;

    public TrackAnalyticsEventCommandHandlerTests()
    {
        _handler = new TrackAnalyticsEventCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldTrackEvent_WhenDataIsValid()
    {
        // Arrange
        var command = AnalyticsTestDataFactory.CreateTrackAnalyticsEventCommand();

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _repositoryMock.Verify(r => r.AddAsync(It.Is<Domain.Entities.Analytics.AnalyticsEvent>(
                e => e.EventType == command.EventType &&
                     e.PageSlug == command.PageSlug &&
                     e.Referrer == command.Referrer &&
                     e.UserAgent == command.UserAgent &&
                     e.AdditionalDataJson == command.AdditionalDataJson),
            It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionIsThrown()
    {
        // Arrange
        var command = AnalyticsTestDataFactory.CreateTrackAnalyticsEventCommand(
            pageSlug: "/error",
            referrer: null,
            userAgent: null,
            additionalDataJson: null
        );

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Domain.Entities.Analytics.AnalyticsEvent>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Simulated failure"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An error occurred while tracking the event.");

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }
}