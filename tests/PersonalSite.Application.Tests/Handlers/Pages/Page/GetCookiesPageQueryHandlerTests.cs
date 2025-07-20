using PersonalSite.Application.Common.Localization;
using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Features.Pages.Page.Queries.GetCookiesPage;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Pages;

namespace PersonalSite.Application.Tests.Handlers.Pages.Page;

public class GetCookiesPageQueryHandlerTests
{
    private readonly Mock<IPageRepository> _pageRepositoryMock;
    private readonly Mock<ITranslatableMapper<Domain.Entities.Pages.Page, PageDto>> _pageMapperMock;
    private readonly LanguageContext _languageContext;
    private readonly GetCookiesPageQueryHandler _handler;

    public GetCookiesPageQueryHandlerTests()
    {
        _pageRepositoryMock = new Mock<IPageRepository>();
        var loggerMock = new Mock<ILogger<GetCookiesPageQueryHandler>>();
        _pageMapperMock = new Mock<ITranslatableMapper<Domain.Entities.Pages.Page, PageDto>>();

        _languageContext = new LanguageContext { LanguageCode = "en" };

        _handler = new GetCookiesPageQueryHandler(
            _languageContext,
            _pageRepositoryMock.Object,
            loggerMock.Object,
            _pageMapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_InvalidLanguageContext_ReturnsFailure()
    {
        // Arrange
        _languageContext.LanguageCode = null!;
        var query = new GetCookiesPageQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Invalid language context.");
    }

    [Fact]
    public async Task Handle_CookiesPageNotFound_ReturnsFailure()
    {
        // Arrange
        _pageRepositoryMock
            .Setup(r => r.GetByKeyAsync("cookies", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Pages.Page?)null);

        // Act
        var result = await _handler.Handle(new GetCookiesPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Cookies page not found.");
    }

    [Fact]
    public async Task Handle_CookiesPageFound_ReturnsSuccess()
    {
        // Arrange
        var page = PageTestDataFactory.CreatePage();
        var pageDto = new PageDto
        {
            Title = "Cookies Title",
            Description = "Cookies Description"
        };

        _pageRepositoryMock
            .Setup(r => r.GetByKeyAsync("cookies", It.IsAny<CancellationToken>()))
            .ReturnsAsync(page);

        _pageMapperMock
            .Setup(m => m.MapToDto(page, "en"))
            .Returns(pageDto);

        // Act
        var result = await _handler.Handle(new GetCookiesPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.PageData.Should().Be(pageDto);
    }

    [Fact]
    public async Task Handle_ExceptionThrown_ReturnsFailureAndLogsError()
    {
        // Arrange
        _pageRepositoryMock
            .Setup(r => r.GetByKeyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB failure"));

        // Act
        var result = await _handler.Handle(new GetCookiesPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");
    }
}