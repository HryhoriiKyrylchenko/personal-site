using PersonalSite.Application.Common.Localization;
using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Pages.Page.Dtos;
using PersonalSite.Application.Features.Pages.Page.Queries.GetHomePage;
using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Application.Features.Skills.UserSkills.Dtos;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Projects;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Pages;
using PersonalSite.Domain.Interfaces.Repositories.Projects;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Pages.Page;

public class GetHomePageQueryHandlerTests
{
    private readonly Mock<IPageRepository> _pageRepositoryMock;
    private readonly Mock<IUserSkillRepository> _userSkillRepositoryMock;
    private readonly Mock<IProjectRepository> _projectRepositoryMock;
    private readonly Mock<ILogger<GetHomePageQueryHandler>> _loggerMock;
    private readonly Mock<ITranslatableMapper<Domain.Entities.Pages.Page, PageDto>> _pageMapperMock;
    private readonly Mock<ITranslatableMapper<UserSkill, UserSkillDto>> _userSkillMapperMock;
    private readonly Mock<ITranslatableMapper<Project, ProjectDto>> _projectMapperMock;
    private readonly LanguageContext _languageContext;
    private readonly GetHomePageQueryHandler _handler;

    public GetHomePageQueryHandlerTests()
    {
        _pageRepositoryMock = new Mock<IPageRepository>();
        _userSkillRepositoryMock = new Mock<IUserSkillRepository>();
        _projectRepositoryMock = new Mock<IProjectRepository>();
        _loggerMock = new Mock<ILogger<GetHomePageQueryHandler>>();
        _pageMapperMock = new Mock<ITranslatableMapper<Domain.Entities.Pages.Page, PageDto>>();
        _userSkillMapperMock = new Mock<ITranslatableMapper<UserSkill, UserSkillDto>>();
        _projectMapperMock = new Mock<ITranslatableMapper<Project, ProjectDto>>();

        _languageContext = new LanguageContext { LanguageCode = "en" };

        _handler = new GetHomePageQueryHandler(
            _languageContext,
            _pageRepositoryMock.Object,
            _userSkillRepositoryMock.Object,
            _projectRepositoryMock.Object,
            _loggerMock.Object,
            _pageMapperMock.Object,
            _userSkillMapperMock.Object,
            _projectMapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_InvalidLanguageContext_ReturnsFailure()
    {
        // Arrange
        _languageContext.LanguageCode = null!;
        var query = new GetHomePageQuery();

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
        _pageRepositoryMock.Setup(r => r.GetByKeyAsync("home", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Domain.Entities.Pages.Page?)null);

        // Act
        var result = await _handler.Handle(new GetHomePageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("About page not found.");
    }

    [Fact]
    public async Task Handle_PageFound_NoSkills_NoProject_ReturnsSuccess()
    {
        // Arrange
        var page = PageTestDataFactory.CreatePage();
        var pageDto = new PageDto { Title = "Home" };

        _pageRepositoryMock.Setup(r => r.GetByKeyAsync("home", It.IsAny<CancellationToken>()))
            .ReturnsAsync(page);
        _pageMapperMock.Setup(m => m.MapToDto(page, "en")).Returns(pageDto);

        _userSkillRepositoryMock.Setup(r => r.GetAllActiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<UserSkill>());
        _userSkillMapperMock.Setup(m => m.MapToDtoList(It.IsAny<IReadOnlyList<UserSkill>>(), "en"))
            .Returns(new List<UserSkillDto>());

        _projectRepositoryMock.Setup(r => r.GetLastAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync((Project?)null);

        // Act
        var result = await _handler.Handle(new GetHomePageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.PageData.Should().Be(pageDto);
        result.Value.UserSkills.Should().BeEmpty();
        result.Value.LastProject.Should().BeNull();
    }

    [Fact]
    public async Task Handle_AllDataPresent_ReturnsSuccess()
    {
        // Arrange
        var page = PageTestDataFactory.CreatePage();
        var pageDto = new PageDto { Title = "Home" };

        var userSkill = new UserSkill { Id = Guid.NewGuid() };
        var userSkillDto = new UserSkillDto { Id = userSkill.Id };

        var project = new Project { Id = Guid.NewGuid() };
        var projectDto = new ProjectDto { Id = project.Id };

        _pageRepositoryMock.Setup(r => r.GetByKeyAsync("home", It.IsAny<CancellationToken>()))
            .ReturnsAsync(page);
        _pageMapperMock.Setup(m => m.MapToDto(page, "en")).Returns(pageDto);

        _userSkillRepositoryMock.Setup(r => r.GetAllActiveAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync([userSkill]);
        _userSkillMapperMock.Setup(m => m.MapToDtoList(It.IsAny<IReadOnlyList<UserSkill>>(), "en"))
            .Returns([userSkillDto]);

        _projectRepositoryMock.Setup(r => r.GetLastAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(project);
        _projectMapperMock.Setup(m => m.MapToDto(project, "en")).Returns(projectDto);

        // Act
        var result = await _handler.Handle(new GetHomePageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value!.PageData.Should().Be(pageDto);
        result.Value.UserSkills.Should().ContainSingle();
        result.Value.LastProject.Should().Be(projectDto);
    }

    [Fact]
    public async Task Handle_ExceptionThrown_ReturnsFailure()
    {
        // Arrange
        _pageRepositoryMock.Setup(r => r.GetByKeyAsync("home", It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB crash"));

        // Act
        var result = await _handler.Handle(new GetHomePageQuery(), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("An unexpected error occurred.");

        _loggerMock.Verify(
            l => l.Log(
                LogLevel.Error,
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, _) => v.ToString()!.Contains("Error occurred while retrieving home page data.")),
                It.IsAny<Exception>(),
                It.IsAny<Func<It.IsAnyType, Exception?, string>>()
            ),
            Times.Once);
    }
}