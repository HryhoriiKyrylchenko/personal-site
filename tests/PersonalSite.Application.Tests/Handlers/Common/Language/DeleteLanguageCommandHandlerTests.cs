using PersonalSite.Application.Features.Common.Language.Commands.DeleteLanguage;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.Language;

public class DeleteLanguageCommandHandlerTests
{
    private readonly Mock<ILanguageRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<DeleteLanguageCommandHandler>> _loggerMock = new();
    private readonly DeleteLanguageCommandHandler _handler;

    public DeleteLanguageCommandHandlerTests()
    {
        _handler = new DeleteLanguageCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldSoftDeleteLanguage_WhenLanguageExists()
    {
        // Arrange
        var language = new Domain.Entities.Common.Language
        {
            Id = Guid.NewGuid(),
            IsDeleted = false
        };
        var command = new DeleteLanguageCommand(language.Id);

        _repositoryMock.Setup(r => r.GetByIdAsync(language.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(language);

        _repositoryMock.Setup(r => r.UpdateAsync(language, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        language.IsDeleted.Should().BeTrue();

        _repositoryMock.Verify(r => r.UpdateAsync(language, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenLanguageNotFound()
    {
        // Arrange
        var command = new DeleteLanguageCommand(Guid.NewGuid());

        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Common.Language?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Language not found.");

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Language not found.")),
                null, // Exception is null for warnings here
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Common.Language>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_OnException()
    {
        // Arrange
        var command = new DeleteLanguageCommand(Guid.NewGuid());

        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB failure"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Error occurred while deleting language");

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error occurred while deleting language")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()), 
            Times.Once);
    }
}