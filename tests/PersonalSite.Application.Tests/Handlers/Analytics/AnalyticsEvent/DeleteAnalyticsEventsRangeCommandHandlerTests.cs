using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.DeleteAnalyticsEventsRange;
using PersonalSite.Domain.Interfaces.Repositories.Analytics;

namespace PersonalSite.Application.Tests.Handlers.Analytics.AnalyticsEvent;

public class DeleteAnalyticsEventsRangeCommandHandlerTests
{
    private readonly Mock<IAnalyticsEventRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<DeleteAnalyticsEventsRangeCommandHandler>> _loggerMock = new();

    private readonly DeleteAnalyticsEventsRangeCommandHandler _handler;

    public DeleteAnalyticsEventsRangeCommandHandlerTests()
    {
        _handler = new DeleteAnalyticsEventsRangeCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ShouldDeleteAllEvents_WhenAllExist()
    {
        // Arrange
        var ids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };

        foreach (var id in ids)
        {
            var entity = new Domain.Entities.Analytics.AnalyticsEvent { Id = id };
            _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(entity);
        }

        var command = new DeleteAnalyticsEventsRangeCommand(ids);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        foreach (var id in ids)
        {
            _repositoryMock.Verify(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()), Times.Once);
            _repositoryMock.Verify(r => r.Remove(It.IsAny<Domain.Entities.Analytics.AnalyticsEvent>()), Times.Exactly(ids.Count));

        }
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldDeleteOnlyExistingEvents_WhenSomeAreMissing()
    {
        // Arrange
        var existingId = Guid.NewGuid();
        var missingId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetByIdAsync(existingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Domain.Entities.Analytics.AnalyticsEvent { Id = existingId });

        _repositoryMock.Setup(r => r.GetByIdAsync(missingId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Analytics.AnalyticsEvent?)null);

        var command = new DeleteAnalyticsEventsRangeCommand([existingId, missingId]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.Remove(It.Is<Domain.Entities.Analytics.AnalyticsEvent>(e => e.Id == existingId)), Times.Once);
        _repositoryMock.Verify(r => r.Remove(It.Is<Domain.Entities.Analytics.AnalyticsEvent>(e => e.Id == missingId)), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionIsThrown()
    {
        // Arrange
        var id = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("boom"));

        var command = new DeleteAnalyticsEventsRangeCommand([id]);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An error occurred while deleting the event.");
    }
}