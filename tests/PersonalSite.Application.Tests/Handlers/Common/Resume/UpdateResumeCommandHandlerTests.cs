using PersonalSite.Application.Features.Common.Resume.Commands.UpdateResume;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.Resume;

public class UpdateResumeCommandHandlerTests
{
    private readonly Mock<IResumeRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<UpdateResumeCommandHandler>> _loggerMock;
    private readonly UpdateResumeCommandHandler _handler;

    public UpdateResumeCommandHandlerTests()
    {
        _repositoryMock = new Mock<IResumeRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<UpdateResumeCommandHandler>>();

        _handler = new UpdateResumeCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenResumeExistsAndUpdated()
    {
        // Arrange
        var resumeId = Guid.NewGuid();
        var existingResume = new Domain.Entities.Common.Resume
        {
            Id = resumeId,
            FileUrl = "old-url",
            FileName = "old-file.txt",
            IsActive = false
        };

        var command = new UpdateResumeCommand(
            resumeId,
            FileUrl: "new-url",
            FileName: "new-file.txt",
            IsActive: true);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(resumeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingResume);

        _repositoryMock
            .Setup(r => r.UpdateAsync(existingResume, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        existingResume.FileUrl.Should().Be("new-url");
        existingResume.FileName.Should().Be("new-file.txt");
        existingResume.IsActive.Should().BeTrue();

        _repositoryMock.Verify(r => r.GetByIdAsync(resumeId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(existingResume, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenResumeNotFound()
    {
        // Arrange
        var resumeId = Guid.NewGuid();
        var command = new UpdateResumeCommand(resumeId, "url", "file", true);

        _repositoryMock
            .Setup(r => r.GetByIdAsync(resumeId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Common.Resume?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Resume not found.");

        _repositoryMock.Verify(r => r.GetByIdAsync(resumeId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Common.Resume>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        _loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_AndLogError_WhenExceptionThrown()
    {
        // Arrange
        var resumeId = Guid.NewGuid();
        var command = new UpdateResumeCommand(resumeId, "url", "file", true);
        var exception = new Exception("DB failure");

        _repositoryMock
            .Setup(r => r.GetByIdAsync(resumeId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Failed to update resume.");

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((state, _) => state.ToString()!.Contains("Error updating resume.")),
                exception,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}