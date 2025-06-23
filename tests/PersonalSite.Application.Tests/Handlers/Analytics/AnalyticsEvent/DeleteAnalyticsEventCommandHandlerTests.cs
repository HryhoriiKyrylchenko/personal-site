using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.DeleteAnalyticsEvent;
using PersonalSite.Domain.Interfaces.Repositories.Analytics;

namespace PersonalSite.Application.Tests.Handlers.Analytics.AnalyticsEvent;

public class DeleteAnalyticsEventCommandHandlerTests
    {
        private readonly Mock<IAnalyticsEventRepository> _repositoryMock = new();
        private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
        private readonly Mock<ILogger<DeleteAnalyticsEventCommandHandler>> _loggerMock = new();

        private readonly DeleteAnalyticsEventCommandHandler _handler;

        public DeleteAnalyticsEventCommandHandlerTests()
        {
            _handler = new DeleteAnalyticsEventCommandHandler(
                _repositoryMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldDeleteEvent_WhenEventExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var analyticsEvent = new Domain.Entities.Analytics.AnalyticsEvent { Id = id };

            _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(analyticsEvent);

            // Act
            var result = await _handler.Handle(new DeleteAnalyticsEventCommand(id), CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            _repositoryMock.Verify(r => r.Remove(analyticsEvent), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenEventNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Analytics.AnalyticsEvent?)null);

            // Act
            var result = await _handler.Handle(new DeleteAnalyticsEventCommand(id), CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("AnalyticsEvent not found.");

            _repositoryMock.Verify(r => r.Remove(It.IsAny<Domain.Entities.Analytics.AnalyticsEvent>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenExceptionIsThrown()
        {
            // Arrange
            var id = Guid.NewGuid();

            _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("boom"));

            // Act
            var result = await _handler.Handle(new DeleteAnalyticsEventCommand(id), CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("An error occurred while deleting the event.");
        }
    }