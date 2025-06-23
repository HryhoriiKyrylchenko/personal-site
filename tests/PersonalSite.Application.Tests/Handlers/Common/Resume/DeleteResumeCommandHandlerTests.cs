using PersonalSite.Application.Features.Common.Resume.Commands.DeleteResume;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.Resume;

public class DeleteResumeCommandHandlerTests
{
    private readonly Mock<IResumeRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<DeleteResumeCommandHandler>> _loggerMock;
    private readonly DeleteResumeCommandHandler _handler;

    public DeleteResumeCommandHandlerTests()
    {
        _repositoryMock = new Mock<IResumeRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<DeleteResumeCommandHandler>>();

        _handler = new DeleteResumeCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenResumeExistsAndDeleted()
    {
        // Arrange
        var resumeId = Guid.NewGuid();
        var resume = new Domain.Entities.Common.Resume { Id = resumeId };

        _repositoryMock
            .Setup(r => r.GetByIdAsync(resumeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(resume);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(new DeleteResumeCommand(resumeId), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        _repositoryMock.Verify(r => r.GetByIdAsync(resumeId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.Remove(resume), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenResumeNotFound()
    {
        // Arrange
        var resumeId = Guid.NewGuid();

        _repositoryMock
            .Setup(r => r.GetByIdAsync(resumeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Common.Resume?)null);

        // Act
        var result = await _handler.Handle(new DeleteResumeCommand(resumeId), CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Resume not found.");

        _repositoryMock.Verify(r => r.GetByIdAsync(resumeId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.Remove(It.IsAny<Domain.Entities.Common.Resume>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_AndLogError_WhenExceptionThrown()
    {
        // Arrange
        var resumeId = Guid.NewGuid();
        var exception = new Exception("Database failure");

        _repositoryMock
            .Setup(r => r.GetByIdAsync(resumeId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _handler.Handle(new DeleteResumeCommand(resumeId), CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Failed to delete resume.");

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) => state.ToString()!.Contains("Error deleting resume.")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}