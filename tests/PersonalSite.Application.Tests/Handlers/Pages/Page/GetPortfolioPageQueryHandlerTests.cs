using PersonalSite.Application.Common.Localization;
using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Features.Pages.Page.Queries.GetPortfolioPage;
using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Interfaces.Repositories.Pages;
using PersonalSite.Domain.Interfaces.Repositories.Projects;

namespace PersonalSite.Application.Tests.Handlers.Pages.Page;

public class GetPortfolioPageQueryHandlerTests
{
    private readonly Mock<IPageRepository> _pageRepositoryMock;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<ITranslatableMapper<Domain.Entities.Pages.Page, PageDto>> _pageMapperMock;
    private readonly Mock<ITranslatableMapper<Domain.Entities.Projects.Project, ProjectDto>> _projectMapperMock;
    private readonly LanguageContext _languageContext;
    private readonly GetPortfolioPageQueryHandler _handler;

    public GetPortfolioPageQueryHandlerTests()
    {
        _pageRepositoryMock = new Mock<IPageRepository>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
        var loggerMock = new Mock<ILogger<GetPortfolioPageQueryHandler>>();
        _pageMapperMock = new Mock<ITranslatableMapper<Domain.Entities.Pages.Page, PageDto>>();
        _projectMapperMock = new Mock<ITranslatableMapper<Domain.Entities.Projects.Project, ProjectDto>>();

        _languageContext = new LanguageContext { LanguageCode = "en" };

        _handler = new GetPortfolioPageQueryHandler(
            _languageContext,
            _pageRepositoryMock.Object,
            _projectRepositoryMock.Object,
            loggerMock.Object,
            _pageMapperMock.Object,
            _projectMapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_InvalidLanguageContext_ReturnsFailure()
    {
        // Arrange
        _languageContext.LanguageCode = null!;
        var query = new GetPortfolioPageQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Invalid language context.");
    }

    [Fact]
    public async Task Handle_PageNotFound_ReturnsFailure()
    {
        // Arrange
        var query = new GetPortfolioPageQuery();

        _pageRepositoryMock.Setup(r => r.GetByKeyAsync("portfolio", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Pages.Page?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Portfolio page not found.");
    }

    [Fact]
    public async Task Handle_NoProjects_ReturnsSuccessWithEmptyList()
    {
        // Arrange
        var page = PageTestDataFactory.CreatePage();
        var pageDto = PageTestDataFactory.MapToDto(page);
        var query = new GetPortfolioPageQuery();

        _pageRepositoryMock.Setup(r => r.GetByKeyAsync("portfolio", It.IsAny<CancellationToken>()))
            .ReturnsAsync(page);

        _pageMapperMock.Setup(m => m.MapToDto(page, _languageContext.LanguageCode))
            .Returns(pageDto);

        _projectRepositoryMock.Setup(r => r.GetAllWithFullDataAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Domain.Entities.Projects.Project>());

        _projectMapperMock.Setup(m => m.MapToDtoList(It.IsAny<List<Domain.Entities.Projects.Project>>(), _languageContext.LanguageCode))
            .Returns(new List<ProjectDto>());

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeNull();
        result.Value!.Projects.Should().BeEmpty();
        result.Value.PageData.Should().NotBeNull();
    }

    [Fact]
    public async Task Handle_PageAndProjectsFound_ReturnsSuccess()
    {
        // Arrange
        var page = PageTestDataFactory.CreatePage();
        var pageDto = PageTestDataFactory.MapToDto(page);

        var project = new Domain.Entities.Projects.Project { Id = Guid.NewGuid() };
        var projectDto = new ProjectDto { Id = project.Id };

        _pageRepositoryMock.Setup(r => r.GetByKeyAsync("portfolio", It.IsAny<CancellationToken>()))
            .ReturnsAsync(page);

        _pageMapperMock.Setup(m => m.MapToDto(page, _languageContext.LanguageCode))
            .Returns(pageDto);

        _projectRepositoryMock.Setup(r => r.GetAllWithFullDataAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([project]);

        _projectMapperMock.Setup(m => m.MapToDtoList(It.IsAny<IReadOnlyList<Domain.Entities.Projects.Project>>(), _languageContext.LanguageCode))
            .Returns([projectDto]);

        // Act
        var result = await _handler.Handle(new GetPortfolioPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.PageData.Should().Be(pageDto);
        result.Value.Projects.Should().ContainSingle();
    }

    [Fact]
    public async Task Handle_ExceptionThrown_ReturnsFailure()
    {
        // Arrange
        _pageRepositoryMock.Setup(r => r.GetByKeyAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB failure"));

        // Act
        var result = await _handler.Handle(new GetPortfolioPageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");
    }
}