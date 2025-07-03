using PersonalSite.Application.Features.Common.Language.Commands.UpdateLanguage;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Common;

namespace PersonalSite.Application.Tests.Handlers.Common.Language;

public class UpdateLanguageCommandHandlerTests
{
    private readonly Mock<ILanguageRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly UpdateLanguageCommandHandler _handler;

    public UpdateLanguageCommandHandlerTests()
    {
        _repositoryMock = new Mock<ILanguageRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        var loggerMock = new Mock<ILogger<UpdateLanguageCommandHandler>>();
        _handler = new UpdateLanguageCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenLanguageNotFound()
    {
        // Arrange
        var command = CommonTestDataFactory.CreateUpdateLanguageCommand();
        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Common.Language?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Language not found.");

        _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<Domain.Entities.Common.Language>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldUpdateLanguage_WhenLanguageExists()
    {
        // Arrange
        var language = CommonTestDataFactory.CreateLanguage("fr");

        var command = CommonTestDataFactory.CreateUpdateLanguageCommand(language.Id);

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
        var command = CommonTestDataFactory.CreateUpdateLanguageCommand();
        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Error occurred while updating language");
    }
}