using PersonalSite.Application.Features.Skills.Skills.Commands.CreateSkill;
using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

namespace PersonalSite.Application.Tests.Handlers.Skills.Skills;

public class CreateSkillCommandHandlerTests
{
    private readonly Mock<ISkillRepository> _skillRepoMock = new();
    private readonly Mock<ISkillCategoryRepository> _categoryRepoMock = new();
    private readonly Mock<ISkillTranslationRepository> _translationRepoMock = new();
    private readonly Mock<ILanguageRepository> _languageRepoMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<CreateSkillCommandHandler>> _loggerMock = new();

    private readonly CreateSkillCommandHandler _handler;

    public CreateSkillCommandHandlerTests()
    {
        _handler = new CreateSkillCommandHandler(
            _skillRepoMock.Object,
            _categoryRepoMock.Object,
            _translationRepoMock.Object,
            _languageRepoMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateSkill_WhenValid()
    {
        // Arrange
        var command = new CreateSkillCommand(
            Guid.NewGuid(),
            "csharp",
            new List<SkillTranslationDto> { SkillsTestDataFactory.CreateTranslationDto() });

        _skillRepoMock.Setup(r => r.ExistsByKeyAsync(command.Key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _categoryRepoMock.Setup(r => r.GetByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SkillCategory { Id = command.CategoryId });

        _languageRepoMock.Setup(r => r.GetByCodeAsync("en", It.IsAny<CancellationToken>()))
            .ReturnsAsync(CommonTestDataFactory.CreateLanguage());

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        _skillRepoMock.Verify(r => r.AddAsync(It.IsAny<Skill>(), It.IsAny<CancellationToken>()), Times.Once);
        _translationRepoMock.Verify(r => r.AddAsync(It.IsAny<SkillTranslation>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenKeyExists()
    {
        var command = new CreateSkillCommand(Guid.NewGuid(), "csharp", []);

        _skillRepoMock.Setup(r => r.ExistsByKeyAsync(command.Key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Skill key already exists.");
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenCategoryNotFound()
    {
        var command = new CreateSkillCommand(Guid.NewGuid(), "csharp", []);

        _skillRepoMock.Setup(r => r.ExistsByKeyAsync(command.Key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _categoryRepoMock.Setup(r => r.GetByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SkillCategory?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be($"Skill category with ID {command.CategoryId} not found.");
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenLanguageNotFound()
    {
        var command = new CreateSkillCommand(
            Guid.NewGuid(), "csharp", [SkillsTestDataFactory.CreateTranslationDto("uk")]);

        _skillRepoMock.Setup(r => r.ExistsByKeyAsync(command.Key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _categoryRepoMock.Setup(r => r.GetByIdAsync(command.CategoryId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new SkillCategory { Id = command.CategoryId });

        _languageRepoMock.Setup(r => r.GetByCodeAsync("uk", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Language?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Language 'uk' not found.");
    }

    [Fact]
    public async Task Handle_ShouldFail_WhenExceptionThrown()
    {
        var command = new CreateSkillCommand(Guid.NewGuid(), "csharp", []);

        _skillRepoMock.Setup(r => r.ExistsByKeyAsync(command.Key, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error while creating skill.");
    }
}