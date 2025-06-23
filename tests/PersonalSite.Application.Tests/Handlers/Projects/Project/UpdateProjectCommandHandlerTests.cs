using PersonalSite.Application.Features.Projects.Project.Commands.UpdateProject;
using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Projects;
using PersonalSite.Domain.Interfaces.Repositories.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

namespace PersonalSite.Application.Tests.Handlers.Projects.Project;

public class UpdateProjectCommandHandlerTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock = new();
    private readonly Mock<ILanguageRepository> _languageRepositoryMock = new();
    private readonly Mock<IProjectTranslationRepository> _translationRepositoryMock = new();
    private readonly Mock<IProjectSkillRepository> _projectSkillRepositoryMock = new();
    private readonly Mock<ISkillRepository> _skillRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<UpdateProjectCommandHandler>> _loggerMock = new();

    private readonly UpdateProjectCommandHandler _handler;

    public UpdateProjectCommandHandlerTests()
    {
        _handler = new UpdateProjectCommandHandler(
            _projectRepositoryMock.Object,
            _languageRepositoryMock.Object,
            _translationRepositoryMock.Object,
            _projectSkillRepositoryMock.Object,
            _skillRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenProjectNotFound()
    {
        // Arrange
        var command = new UpdateProjectCommand { Id = Guid.NewGuid() };
        _projectRepositoryMock.Setup(r => r.GetWithFullDataAsync(command.Id, CancellationToken.None))
            .ReturnsAsync((Domain.Entities.Projects.Project?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("not found");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSlugAlreadyUsed()
    {
        // Arrange
        var command = new UpdateProjectCommand { Id = Guid.NewGuid(), Slug = "existing-slug" };
        var project = new Domain.Entities.Projects.Project { Id = command.Id, Slug = "old-slug" };

        _projectRepositoryMock.Setup(r => r.GetWithFullDataAsync(command.Id, CancellationToken.None))
            .ReturnsAsync(project);
        _projectRepositoryMock.Setup(r => r.IsSlugAvailableAsync(command.Slug, CancellationToken.None))
            .ReturnsAsync(false);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("slug already exists");
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUpdateSucceeds()
    {
        // Arrange
        var projectId = Guid.NewGuid();
        var skillId = Guid.NewGuid();
        var languageId = Guid.NewGuid();

        var command = new UpdateProjectCommand
        {
            Id = projectId,
            Slug = "updated-slug",
            CoverImage = "new-image.png",
            DemoUrl = "demo.com",
            RepoUrl = "repo.com",
            SkillIds = [skillId],
            Translations = [
                new ProjectTranslationDto
                {
                    LanguageCode = "en",
                    Title = "Title",
                    ShortDescription = "Short desc",
                    DescriptionSections = new(),
                    MetaTitle = "Meta",
                    MetaDescription = "Meta desc",
                    OgImage = "og.png"
                }
            ]
        };

        var language = new Language { Id = languageId, Code = "en" };
        var project = new Domain.Entities.Projects.Project { Id = projectId, Slug = "old-slug", ProjectSkills = [], Translations = [] };

        _projectRepositoryMock.Setup(r => r.GetWithFullDataAsync(projectId, CancellationToken.None))
            .ReturnsAsync(project);
        _projectRepositoryMock.Setup(r => r.IsSlugAvailableAsync("updated-slug", CancellationToken.None))
            .ReturnsAsync(true);

        _languageRepositoryMock.Setup(r => r.GetByCodeAsync("en", CancellationToken.None))
            .ReturnsAsync(language);

        _translationRepositoryMock.Setup(r => r.GetByProjectIdAsync(projectId, CancellationToken.None))
            .ReturnsAsync(new List<ProjectTranslation>());

        _projectSkillRepositoryMock.Setup(r => r.GetByProjectIdAsync(projectId, CancellationToken.None))
            .ReturnsAsync(new List<ProjectSkill>());

        _skillRepositoryMock.Setup(r => r.GetByIdAsync(skillId, CancellationToken.None))
            .ReturnsAsync(new Skill { Id = skillId });

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(CancellationToken.None), Times.Once);
    }
}