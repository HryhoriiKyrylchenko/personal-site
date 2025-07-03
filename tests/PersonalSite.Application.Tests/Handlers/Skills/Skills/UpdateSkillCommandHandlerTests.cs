using PersonalSite.Application.Features.Skills.Skills.Commands.UpdateSkill;
using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

namespace PersonalSite.Application.Tests.Handlers.Skills.Skills;

public class UpdateSkillCommandHandlerTests
{
    private readonly Mock<ISkillRepository> _skillRepoMock = new();
    private readonly Mock<ISkillCategoryRepository> _categoryRepoMock = new();
    private readonly Mock<ISkillTranslationRepository> _translationRepoMock = new();
    private readonly Mock<ILanguageRepository> _languageRepoMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<UpdateSkillCommandHandler>> _loggerMock = new();

    private readonly UpdateSkillCommandHandler _handler;

    public UpdateSkillCommandHandlerTests()
    {
        _handler = new UpdateSkillCommandHandler(
            _skillRepoMock.Object,
            _categoryRepoMock.Object,
            _translationRepoMock.Object,
            _languageRepoMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Succeed_When_Valid()
    {
        // Arrange
        var skill = SkillsTestDataFactory.CreateSkillWithTranslationsAndCategory();
        var translation = skill.Translations.First();
        var language = CommonTestDataFactory.CreateLanguage(translation.Language.Code);

        var command = new UpdateSkillCommand(
            skill.Id,
            skill.CategoryId,
            "updated-key",
            new List<SkillTranslationDto>
            {
                new SkillTranslationDto
                {
                    LanguageCode = language.Code,
                    Name = "Updated Name",
                    Description = "Updated Description"
                }
            });

        _skillRepoMock.Setup(x => x.GetByIdAsync(skill.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(skill);

        _skillRepoMock.Setup(x => x.ExistsByKeyAsync("updated-key", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _translationRepoMock.Setup(x => x.GetBySkillIdAsync(skill.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<SkillTranslation> { translation });

        _languageRepoMock.Setup(x => x.GetByCodeAsync(language.Code, It.IsAny<CancellationToken>()))
            .ReturnsAsync(language);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        skill.Key.Should().Be("updated-key");

        _skillRepoMock.Verify(x => x.UpdateAsync(skill, It.IsAny<CancellationToken>()), Times.Once);
        _translationRepoMock.Verify(x => x.UpdateAsync(It.Is<SkillTranslation>(t => t.Name == "Updated Name"), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_Skill_Not_Found()
    {
        // Arrange
        var command = new UpdateSkillCommand(Guid.NewGuid(), Guid.NewGuid(), "key", []);

        _skillRepoMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Skill?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Skill not found.");
    }

    [Fact]
    public async Task Handle_Should_Fail_When_Key_Exists()
    {
        // Arrange
        var skill = SkillsTestDataFactory.CreateSkill();
        var command = new UpdateSkillCommand(skill.Id, skill.CategoryId, "existing-key", []);

        _skillRepoMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(skill);

        _skillRepoMock.Setup(x => x.ExistsByKeyAsync("existing-key", It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("A skill with this key already exists.");
    }

    [Fact]
    public async Task Handle_Should_Fail_When_Language_Not_Found()
    {
        // Arrange
        var skill = SkillsTestDataFactory.CreateSkill();
        var command = new UpdateSkillCommand(skill.Id, skill.CategoryId, "new-key", new()
        {
            new SkillTranslationDto
            {
                LanguageCode = "de",
                Name = "German Name",
                Description = "German Description"
            }
        });

        _skillRepoMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(skill);

        _skillRepoMock.Setup(x => x.ExistsByKeyAsync("new-key", It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _translationRepoMock.Setup(x => x.GetBySkillIdAsync(skill.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync([]);

        _languageRepoMock.Setup(x => x.GetByCodeAsync("de", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Language?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Language 'de' not found.");
    }

    [Fact]
    public async Task Handle_Should_Fail_On_Exception()
    {
        // Arrange
        var skill = SkillsTestDataFactory.CreateSkill();
        var command = new UpdateSkillCommand(skill.Id, skill.CategoryId, "new-key", []);

        _skillRepoMock.Setup(x => x.GetByIdAsync(skill.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Unexpected"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error while updating skill.");
    }
}