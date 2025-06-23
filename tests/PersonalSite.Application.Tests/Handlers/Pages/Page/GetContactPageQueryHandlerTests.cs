using PersonalSite.Application.Common.Localization;
using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Features.Pages.Page.Queries.GetContactPage;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Pages;

namespace PersonalSite.Application.Tests.Handlers.Pages.Page;

public class GetContactPageQueryHandlerTests
{
    private readonly Mock<IPageRepository> _pageRepositoryMock;
    private readonly Mock<ILogger<GetContactPageQueryHandler>> _loggerMock;
    private readonly Mock<ITranslatableMapper<Domain.Entities.Pages.Page, PageDto>> _pageMapperMock;
    private readonly LanguageContext _languageContext;
    private readonly GetContactPageQueryHandler _handler;

    public GetContactPageQueryHandlerTests()
    {
        _pageRepositoryMock = new Mock<IPageRepository>();
        _loggerMock = new Mock<ILogger<GetContactPageQueryHandler>>();
        _pageMapperMock = new Mock<ITranslatableMapper<Domain.Entities.Pages.Page, PageDto>>();

        _languageContext = new LanguageContext { LanguageCode = "en" };

        _handler = new GetContactPageQueryHandler(
            _languageContext,
            _pageRepositoryMock.Object,
            null!, // blogPostRepository is not used in your handler; pass null or mock if needed
            _loggerMock.Object,
            _pageMapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_InvalidLanguageContext_ReturnsFailure()
    {
        // Arrange
        _languageContext.LanguageCode = null!;
        var query = new GetContactPageQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Invalid language context.");
    }

    [Fact]
    public async Task Handle_ContactPageNotFound_ReturnsFailure()
    {
        // Arrange
        _pageRepositoryMock.Setup(r => r.GetByKeyAsync("contacts", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Pages.Page?)null);

        // Act
        var result = await _handler.Handle(new GetContactPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Contact page not found.");
    }

    [Fact]
    public async Task Handle_ContactPageFound_ReturnsSuccess()
    {
        // Arrange
        var page = PageTestDataFactory.CreatePage();
        var pageDto = new PageDto
        {
            Title = "Contact Title",
            Description = "Contact Description"
        };

        _pageRepositoryMock.Setup(r => r.GetByKeyAsync("contacts", It.IsAny<CancellationToken>()))
            .ReturnsAsync(page);

        _pageMapperMock.Setup(m => m.MapToDto(page, "en"))
            .Returns(pageDto);

        // Act
        var result = await _handler.Handle(new GetContactPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.PageData.Should().Be(pageDto);
    }

    [Fact]
    public async Task Handle_ExceptionThrown_ReturnsFailureAndLogsError()
    {
        // Arrange
        _pageRepositoryMock.Setup(r => r.GetByKeyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB failure"));

        // Act
        var result = await _handler.Handle(new GetContactPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");

        _loggerMock.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Error occurred while retrieving contact page data.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}