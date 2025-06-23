using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Features.Pages.Page.Queries.GetPages;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Pages;

namespace PersonalSite.Application.Tests.Handlers.Pages.Page;

public class GetPagesQueryHandlerTests
{
    private readonly Mock<IPageRepository> _repositoryMock;
    private readonly Mock<ILogger<GetPagesQueryHandler>> _loggerMock;
    private readonly Mock<IAdminMapper<Domain.Entities.Pages.Page, PageAdminDto>> _mapperMock;
    private readonly GetPagesQueryHandler _handler;

    public GetPagesQueryHandlerTests()
    {
        _repositoryMock = new Mock<IPageRepository>();
        _loggerMock = new Mock<ILogger<GetPagesQueryHandler>>();
        _mapperMock = new Mock<IAdminMapper<Domain.Entities.Pages.Page, PageAdminDto>>();
        _handler = new GetPagesQueryHandler(_repositoryMock.Object, _loggerMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoPagesFound()
    {
        // Arrange
        _repositoryMock
            .Setup(r => r.GetAllWithTranslationsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Entities.Pages.Page>());

        // Act
        var result = await _handler.Handle(new GetPagesQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("No pages found.");

        _loggerMock.Verify(
            x => x.Log(
                LogLevel.Warning,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("No pages found.")),
                null,
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenPagesExist()
    {
        // Arrange
        var pages = new List<Domain.Entities.Pages.Page>
        {
            PageTestDataFactory.CreatePage(key: "home"),
            PageTestDataFactory.CreatePage(key: "blog")
        };
            
        var dtos = pages.Select(PageTestDataFactory.MapToAdminDto).ToList();

        _repositoryMock
            .Setup(r => r.GetAllWithTranslationsAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(pages);

        _mapperMock
            .Setup(m => m.MapToAdminDtoList(pages))
            .Returns(dtos);

        // Act
        var result = await _handler.Handle(new GetPagesQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(dtos);
    }
}
