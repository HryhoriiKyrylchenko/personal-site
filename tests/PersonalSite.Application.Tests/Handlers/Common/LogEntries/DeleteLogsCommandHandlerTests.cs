using PersonalSite.Application.Features.Common.LogEntries.Commands.DeleteLogs;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.LogEntries;

public class DeleteLogsCommandHandlerTests
{
    private readonly Mock<ILogEntryRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<DeleteLogsCommandHandler>> _loggerMock = new();
    private readonly DeleteLogsCommandHandler _handler;

    public DeleteLogsCommandHandlerTests()
    {
        _handler = new DeleteLogsCommandHandler(_repositoryMock.Object, _unitOfWorkMock.Object, _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenAllLogsFoundAndDeleted()
    {
        // Arrange
        var ids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var logs = ids.Select(id => new LogEntry { Id = id }).ToList();

        _repositoryMock.Setup(r => r.GetByIdsAsync(ids, It.IsAny<CancellationToken>()))
            .ReturnsAsync(logs);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(new DeleteLogsCommand(ids), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.Remove(It.IsAny<LogEntry>()), Times.Exactly(ids.Count));
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSomeLogsNotFound()
    {
        // Arrange
        var ids = new List<Guid> { Guid.NewGuid(), Guid.NewGuid() };
        var logs = new List<LogEntry>
        {
            new() { Id = ids[0], /* other properties */ }
        };

        _repositoryMock.Setup(r => r.GetByIdsAsync(ids, It.IsAny<CancellationToken>()))
            .ReturnsAsync(logs);

        // Act
        var result = await _handler.Handle(new DeleteLogsCommand(ids), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Some log entries were not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_OnException()
    {
        // Arrange
        var ids = new List<Guid> { Guid.NewGuid() };
        var exception = new Exception("Database error");

        _repositoryMock.Setup(r => r.GetByIdsAsync(ids, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _handler.Handle(new DeleteLogsCommand(ids), CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Failed to delete log entries.");
        _loggerMock.Verify(
            x => x.Log<It.IsAnyType>(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("Failed to delete log entries.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}