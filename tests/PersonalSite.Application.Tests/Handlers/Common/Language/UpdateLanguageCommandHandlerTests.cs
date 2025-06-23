using PersonalSite.Application.Features.Common.Language.Commands.UpdateLanguage;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.Language;

public class UpdateLanguageCommandHandlerTests
{
    private readonly Mock<ILanguageRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<UpdateLanguageCommandHandler>> _loggerMock;
    private readonly UpdateLanguageCommandHandler _handler;

    public UpdateLanguageCommandHandlerTests()
    {
        _repositoryMock = new Mock<ILanguageRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<UpdateLanguageCommandHandler>>();
        _handler = new UpdateLanguageCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenLanguageNotFound()
    {
        // Arrange
        var command = new UpdateLanguageCommand(Guid.NewGuid(), "en", "English");
        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Common.Language?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Language not found.");

        _loggerMock.Verify(l => l.Log(
            LogLevel.Warning,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Language not found.")),
            null,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);

        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Common.Language>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldUpdateLanguage_WhenLanguageExists()
    {
        // Arrange
        var language = new Domain.Entities.Common.Language
        {
            Id = Guid.NewGuid(),
            Code = "fr",
            Name = "French"
        };

        var command = new UpdateLanguageCommand(language.Id, "en", "English");

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
        language.Code.Should().Be(command.Code);
        language.Name.Should().Be(command.Name);

        _repositoryMock.Verify(r => r.UpdateAsync(language, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_OnException()
    {
        // Arrange
        var command = new UpdateLanguageCommand(Guid.NewGuid(), "en", "English");
        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Error occurred while updating language");

        _loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error occurred while updating language")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}