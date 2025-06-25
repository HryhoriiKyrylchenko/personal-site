using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Features.Pages.Page.Queries.GetPageById;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Pages;

namespace PersonalSite.Application.Tests.Handlers.Pages.Page;

public class GetPageByIdQueryHandlerTests
{
    private readonly Mock<IPageRepository> _repositoryMock = new();
    private readonly Mock<ILogger<GetPageByIdQueryHandler>> _loggerMock = new();
    private readonly Mock<IAdminMapper<Domain.Entities.Pages.Page, PageAdminDto>> _mapperMock = new();

    private GetPageByIdQueryHandler CreateHandler()
    {
        return new GetPageByIdQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenPageNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        _repositoryMock.Setup(r => r.GetWithTranslationByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Pages.Page?)null);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetPageByIdQuery(id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Page not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenPageFound()
    {
        // Arrange
        var page = PageTestDataFactory.CreatePage();
        var dto = PageTestDataFactory.MapToAdminDto(page);

        _repositoryMock.Setup(r => r.GetWithTranslationByIdAsync(page.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(page);

        _mapperMock.Setup(m => m.MapToAdminDto(page))
            .Returns(dto);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetPageByIdQuery(page.Id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(dto);
    }

    [Fact]
    public async Task Handle_ShouldLogErrorAndReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        var id = Guid.NewGuid();
        var exception = new Exception("DB error");
        _repositoryMock.Setup(r => r.GetWithTranslationByIdAsync(id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        var handler = CreateHandler();

        // Act
        var result = await handler.Handle(new GetPageByIdQuery(id), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error getting page by id.");
    }
}