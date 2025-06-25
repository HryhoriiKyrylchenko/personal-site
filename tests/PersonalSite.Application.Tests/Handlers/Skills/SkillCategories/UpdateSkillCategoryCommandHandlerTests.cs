using PersonalSite.Application.Features.Skills.SkillCategories.Commands.UpdateSkillCategory;
using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

namespace PersonalSite.Application.Tests.Handlers.Skills.SkillCategories;

public class UpdateSkillCategoryCommandHandlerTests
{
    private readonly Mock<ISkillCategoryRepository> _skillCategoryRepoMock = new();
    private readonly Mock<ILanguageRepository> _languageRepoMock = new();
    private readonly Mock<ISkillCategoryTranslationRepository> _translationRepoMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<UpdateSkillCategoryCommandHandler>> _loggerMock = new();

    private readonly UpdateSkillCategoryCommandHandler _handler;

    public UpdateSkillCategoryCommandHandlerTests()
    {
        _handler = new UpdateSkillCategoryCommandHandler(
            _skillCategoryRepoMock.Object,
            _translationRepoMock.Object,
            _languageRepoMock.Object,
            _translationRepoMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object
        );
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenValidRequest()
    {
        var category = SkillsTestDataFactory.CreateSkillCategory();
        var command = new UpdateSkillCategoryCommand(
            category.Id,
            "backend-updated",
            5,
            category.Translations.Select(t => new SkillCategoryTranslationDto
            {
                Id = t.Id,
                LanguageCode = t.Language.Code,
                Name = "Updated Name",
                Description = "Updated Description"
            }).ToList());

        _skillCategoryRepoMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _skillCategoryRepoMock.Setup(x => x.ExistsByKeyAsync(command.Key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _translationRepoMock.Setup(x => x.GetBySkillCategoryIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category.Translations.ToList());

        _languageRepoMock.Setup(x => x.GetByCodeAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((string code, CancellationToken _) => CommonTestDataFactory.CreateLanguage(code));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeTrue();
        _skillCategoryRepoMock.Verify(x => x.UpdateAsync(category, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenCategoryNotFound()
    {
        var command = new UpdateSkillCategoryCommand(Guid.NewGuid(), "key", 1, []);

        _skillCategoryRepoMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((SkillCategory?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Skill category not found.");
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenKeyAlreadyExists()
    {
        var category = SkillsTestDataFactory.CreateSkillCategory();
        var command = new UpdateSkillCategoryCommand(
            category.Id,
            "existing-key",
            1,
            category.Translations.Select(t => new SkillCategoryTranslationDto
            {
                Id = t.Id,
                LanguageCode = t.Language.Code,
                Name = t.Name,
                Description = t.Description
            }).ToList());

        _skillCategoryRepoMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _skillCategoryRepoMock.Setup(x => x.ExistsByKeyAsync(command.Key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("A skill category with this key already exists.");
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenLanguageNotFound()
    {
        var category = SkillsTestDataFactory.CreateSkillCategory();
        var command = new UpdateSkillCategoryCommand(
            category.Id,
            "backend",
            1,
            category.Translations.Select(t => new SkillCategoryTranslationDto
            {
                Id = t.Id,
                LanguageCode = "xx",
                Name = "Updated",
                Description = "Updated Desc"
            }).ToList());

        _skillCategoryRepoMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category);

        _skillCategoryRepoMock.Setup(x => x.ExistsByKeyAsync(command.Key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _translationRepoMock.Setup(x => x.GetBySkillCategoryIdAsync(category.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(category.Translations.ToList());

        _languageRepoMock.Setup(x => x.GetByCodeAsync("xx", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Language?)null);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Language 'xx' not found.");
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenExceptionThrown()
    {
        var command = new UpdateSkillCategoryCommand(Guid.NewGuid(), "key", 1, []);
        _skillCategoryRepoMock.Setup(x => x.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        var result = await _handler.Handle(command, CancellationToken.None);

        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error occurred while updating skill category.");
    }
}