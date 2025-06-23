using PersonalSite.Application.Features.Projects.Project.Commands.CreateProject;
using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Projects;
using PersonalSite.Domain.Interfaces.Repositories.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

namespace PersonalSite.Application.Tests.Handlers.Projects.Project;

public class CreateProjectCommandHandlerTests
{
    private readonly Mock<IProjectRepository> _projectRepositoryMock = new();
    private readonly Mock<ILanguageRepository> _languageRepositoryMock = new();
    private readonly Mock<IProjectTranslationRepository> _translationRepositoryMock = new();
    private readonly Mock<IProjectSkillRepository> _projectSkillRepositoryMock = new();
    private readonly Mock<ISkillRepository> _skillRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<CreateProjectCommandHandler>> _loggerMock = new();

    private readonly CreateProjectCommandHandler _handler;

    public CreateProjectCommandHandlerTests()
    {
        _handler = new CreateProjectCommandHandler(
            _projectRepositoryMock.Object,
            _languageRepositoryMock.Object,
            _translationRepositoryMock.Object,
            _projectSkillRepositoryMock.Object,
            _skillRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenSlugNotAvailable()
    {
        var command = new CreateProjectCommand { Slug = "existing-slug" };
        _projectRepositoryMock.Setup(r => r.IsSlugAvailableAsync(command.Slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Slug is already in use.");
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenLanguageNotFound()
    {
        var command = new CreateProjectCommand
        {
            Slug = "new-slug",
            Translations =
            [
                new()
                {
                    LanguageCode = "en", Title = "Title", ShortDescription = "", DescriptionSections = new(),
                    MetaTitle = "", MetaDescription = "", OgImage = ""
                }
            ]
        };

        _projectRepositoryMock.Setup(r => r.IsSlugAvailableAsync(command.Slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _languageRepositoryMock.Setup(l => l.GetByCodeAsync("en", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Language?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Language 'en' not found.");
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenSkillNotFound()
    {
        var command = new CreateProjectCommand
        {
            Slug = "new-slug",
            Translations =
            [
                new()
                {
                    LanguageCode = "en", Title = "Title", ShortDescription = "", DescriptionSections = new(),
                    MetaTitle = "", MetaDescription = "", OgImage = ""
                }
            ],
            SkillIds = [Guid.NewGuid()]
        };

        _projectRepositoryMock.Setup(r => r.IsSlugAvailableAsync(command.Slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _languageRepositoryMock.Setup(l => l.GetByCodeAsync("en", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Language { Id = Guid.NewGuid(), Code = "en" });

        _skillRepositoryMock.Setup(s => s.GetByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((Skill?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsFailure.Should().BeTrue();
        result.Error.Should().Contain("Skill with ID");
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenValid()
    {
        var skillId = Guid.NewGuid();
        var command = new CreateProjectCommand
        {
            Slug = "valid-slug",
            Translations =
            [
                new()
                {
                    LanguageCode = "en", Title = "Valid Title", ShortDescription = "", DescriptionSections = new(),
                    MetaTitle = "", MetaDescription = "", OgImage = ""
                }
            ],
            SkillIds = [skillId]
        };

        _projectRepositoryMock.Setup(r => r.IsSlugAvailableAsync(command.Slug, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        _languageRepositoryMock.Setup(l => l.GetByCodeAsync("en", It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Language { Id = Guid.NewGuid(), Code = "en" });

        _skillRepositoryMock.Setup(s => s.GetByIdAsync(skillId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Skill { Id = skillId });

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);
    }
}