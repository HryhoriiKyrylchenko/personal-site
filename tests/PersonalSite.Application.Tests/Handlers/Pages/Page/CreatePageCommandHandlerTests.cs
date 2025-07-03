using PersonalSite.Application.Features.Pages.Page.Commands.CreatePage;
using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Pages;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

namespace PersonalSite.Application.Tests.Handlers.Pages.Page;

public class CreatePageCommandHandlerTests
{
    private readonly Mock<IPageRepository> _pageRepositoryMock = new();
    private readonly Mock<ILanguageRepository> _languageRepositoryMock = new();
    private readonly Mock<IPageTranslationRepository> _translationRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<CreatePageCommandHandler>> _loggerMock = new();

    private CreatePageCommandHandler CreateHandler() => new(
        _pageRepositoryMock.Object,
        _languageRepositoryMock.Object,
        _translationRepositoryMock.Object,
        _unitOfWorkMock.Object,
        _loggerMock.Object
    );

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenKeyIsNotAvailable()
    {
        // Arrange
        var command = PageTestDataFactory.CreateCreatePageCommand();
        _pageRepositoryMock.Setup(r => r.IsKeyAvailableAsync(command.Key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Key is already in use.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenLanguageNotFound()
    {
        // Arrange
        var command = PageTestDataFactory.CreateCreatePageCommand();
        _pageRepositoryMock.Setup(r => r.IsKeyAvailableAsync(command.Key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _languageRepositoryMock.Setup(r => r.GetByCodeAsync("en", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Language?)null); // Language not found

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Language en not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenPageCreated()
    {
        // Arrange
        var command = PageTestDataFactory.CreateCreatePageCommand();
        var language = CommonTestDataFactory.CreateLanguage();

        _pageRepositoryMock.Setup(r => r.IsKeyAvailableAsync(command.Key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _languageRepositoryMock.Setup(r => r.GetByCodeAsync("en", It.IsAny<CancellationToken>()))
            .ReturnsAsync(language);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);

        _pageRepositoryMock.Verify(r => r.AddAsync(It.IsAny<Domain.Entities.Pages.Page>(), It.IsAny<CancellationToken>()), Times.Once);
        _translationRepositoryMock.Verify(r => r.AddAsync(It.IsAny<PageTranslation>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        var command = PageTestDataFactory.CreateCreatePageCommand();
        _pageRepositoryMock.Setup(r => r.IsKeyAvailableAsync(command.Key, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Unexpected"));

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error creating about page.");
    }
}