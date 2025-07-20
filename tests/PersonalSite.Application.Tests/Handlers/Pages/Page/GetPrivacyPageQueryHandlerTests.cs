using PersonalSite.Application.Common.Localization;
using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Features.Pages.Page.Queries.GetPrivacyPage;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Pages;

namespace PersonalSite.Application.Tests.Handlers.Pages.Page;

public class GetPrivacyPageQueryHandlerTests
{
    private readonly Mock<IPageRepository> _pageRepositoryMock;
    private readonly Mock<ITranslatableMapper<Domain.Entities.Pages.Page, PageDto>> _pageMapperMock;
    private readonly LanguageContext _languageContext;
    private readonly GetPrivacyPageQueryHandler _handler;

    public GetPrivacyPageQueryHandlerTests()
    {
        _pageRepositoryMock = new Mock<IPageRepository>();
        var loggerMock = new Mock<ILogger<GetPrivacyPageQueryHandler>>();
        _pageMapperMock = new Mock<ITranslatableMapper<Domain.Entities.Pages.Page, PageDto>>();

        _languageContext = new LanguageContext { LanguageCode = "en" };

        _handler = new GetPrivacyPageQueryHandler(
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
        var query = new GetPrivacyPageQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Invalid language context.");
    }

    [Fact]
    public async Task Handle_PrivacyPageNotFound_ReturnsFailure()
    {
        // Arrange
        _pageRepositoryMock
            .Setup(r => r.GetByKeyAsync("privacy", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Pages.Page?)null);

        // Act
        var result = await _handler.Handle(new GetPrivacyPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Privacy page not found.");
    }

    [Fact]
    public async Task Handle_PrivacyPageFound_ReturnsSuccess()
    {
        // Arrange
        var page = PageTestDataFactory.CreatePage();
        var pageDto = new PageDto
        {
            Title = "Privacy Title",
            Description = "Privacy Description"
        };

        _pageRepositoryMock
            .Setup(r => r.GetByKeyAsync("privacy", It.IsAny<CancellationToken>()))
            .ReturnsAsync(page);

        _pageMapperMock
            .Setup(m => m.MapToDto(page, "en"))
            .Returns(pageDto);

        // Act
        var result = await _handler.Handle(new GetPrivacyPageQuery(), CancellationToken.None);

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
        var result = await _handler.Handle(new GetPrivacyPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");
    }
}