using PersonalSite.Application.Features.Pages.Page.Commands.UpdatePage;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Pages;
using PersonalSite.Domain.Interfaces.Repositories.Translations;
using PersonalSite.Infrastructure.Storage;

namespace PersonalSite.Application.Tests.Handlers.Pages.Page;

public class UpdatePageCommandHandlerTests
    {
        private readonly Mock<IPageRepository> _pageRepositoryMock;
        private readonly Mock<ILanguageRepository> _languageRepositoryMock;
        private readonly Mock<IPageTranslationRepository> _translationRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IS3UrlBuilder> _urlBuilderMock = new();
        private readonly UpdatePageCommandHandler _handler;

        public UpdatePageCommandHandlerTests()
        {
            _pageRepositoryMock = new Mock<IPageRepository>();
            _languageRepositoryMock = new Mock<ILanguageRepository>();
            _translationRepositoryMock = new Mock<IPageTranslationRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            var loggerMock = new Mock<ILogger<UpdatePageCommandHandler>>();

            _handler = new UpdatePageCommandHandler(
                _pageRepositoryMock.Object,
                _languageRepositoryMock.Object,
                _translationRepositoryMock.Object,
                _unitOfWorkMock.Object,
                loggerMock.Object,
                _urlBuilderMock.Object
            );
        }

        [Fact]
        public async Task Handle_PageExistsAndIsUpdatedSuccessfully_ReturnsSuccess()
        {
            // Arrange
            var page = PageTestDataFactory.CreatePage();
            var command = PageTestDataFactory.CreateUpdatePageCommand(page);

            _pageRepositoryMock.Setup(r => r.GetWithTranslationByIdAsync(page.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(page);

            _pageRepositoryMock.Setup(r => r.IsKeyAvailableAsync(page.Key, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _translationRepositoryMock.Setup(r => r.GetAllByPageKeyAsync(page.Key, It.IsAny<CancellationToken>()))
                .ReturnsAsync(page.Translations.ToList());

            foreach (var translation in page.Translations)
            {
                _languageRepositoryMock
                    .Setup(r => r.GetByCodeAsync(translation.Language.Code, It.IsAny<CancellationToken>()))
                    .ReturnsAsync(translation.Language);
            }

            _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(1);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
        }

        [Fact]
        public async Task Handle_PageNotFound_ReturnsFailure()
        {
            // Arrange
            var command = PageTestDataFactory.CreateUpdatePageCommand(Guid.NewGuid(), "key", []);

            _pageRepositoryMock.Setup(r => r.GetWithTranslationByIdAsync(command.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((Domain.Entities.Pages.Page?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Page not found.");
        }

        [Fact]
        public async Task Handle_DuplicateKey_ReturnsFailure()
        {
            // Arrange
            var page = PageTestDataFactory.CreatePage();
            var newKey = "existing-key";
            var command = PageTestDataFactory.CreateUpdatePageCommand(page.Id, newKey, []);

            _pageRepositoryMock.Setup(r => r.GetWithTranslationByIdAsync(page.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(page);

            _pageRepositoryMock.Setup(r => r.IsKeyAvailableAsync(newKey, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("A page with this key already exists.");
        }

        [Fact]
        public async Task Handle_LanguageNotFound_ReturnsFailure()
        {
            // Arrange
            var page = PageTestDataFactory.CreatePage();
            var dto = PageTestDataFactory.CreatePageTranslationDto
            (
                languageCode: "xx", // non-existent language
                title: "Title",
                data: [],
                description: "desc",
                metaTitle: "meta",
                metaDescription: "meta-desc",
                ogImage: "image.png"
            );

            var command = new UpdatePageCommand(page.Id, page.Key, [dto]);

            _pageRepositoryMock.Setup(r => r.GetWithTranslationByIdAsync(page.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(page);

            _pageRepositoryMock.Setup(r => r.IsKeyAvailableAsync(page.Key, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _translationRepositoryMock.Setup(r => r.GetAllByPageKeyAsync(page.Key, It.IsAny<CancellationToken>()))
                .ReturnsAsync([]);

            _languageRepositoryMock.Setup(r => r.GetByCodeAsync("xx", It.IsAny<CancellationToken>()))
                .ReturnsAsync((Language?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Language xx not found.");
        }

        [Fact]
        public async Task Handle_ExceptionThrown_ReturnsFailure()
        {
            // Arrange
            var page = PageTestDataFactory.CreatePage();
            var command = PageTestDataFactory.CreateUpdatePageCommand(page.Id, page.Key, []);

            _pageRepositoryMock.Setup(r => r.GetWithTranslationByIdAsync(page.Id, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("DB Failure"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be($"Error updating {page.Key} page.");
        }
    }