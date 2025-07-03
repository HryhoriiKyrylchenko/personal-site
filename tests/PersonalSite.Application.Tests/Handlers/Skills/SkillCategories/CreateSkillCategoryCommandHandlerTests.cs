using PersonalSite.Application.Features.Skills.SkillCategories.Commands.CreateSkillCategory;
using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Domain.Interfaces.Repositories.Common;
using PersonalSite.Domain.Interfaces.Repositories.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Translations;

namespace PersonalSite.Application.Tests.Handlers.Skills.SkillCategories;

public class CreateSkillCategoryCommandHandlerTests
{
    private readonly Mock<ISkillCategoryRepository> _categoryRepoMock = new();
    private readonly Mock<ISkillCategoryTranslationRepository> _translationRepoMock = new();
    private readonly Mock<ILanguageRepository> _languageRepoMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<CreateSkillCategoryCommandHandler>> _loggerMock = new();

    private readonly CreateSkillCategoryCommandHandler _handler;

    public CreateSkillCategoryCommandHandlerTests()
    {
        _handler = new CreateSkillCategoryCommandHandler(
            _categoryRepoMock.Object,
            _translationRepoMock.Object,
            _languageRepoMock.Object,
            _translationRepoMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenValid()
    {
        // Arrange
        var lang = CommonTestDataFactory.CreateLanguage();
        var command = new CreateSkillCategoryCommand(
            Key: "backend",
            DisplayOrder: 1,
            Translations: new List<SkillCategoryTranslationDto>
            {
                new()
                {
                    LanguageCode = lang.Code,
                    Name = "Backend",
                    Description = "Backend desc"
                }
            });

        _categoryRepoMock.Setup(r => r.ExistsByKeyAsync(command.Key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _languageRepoMock.Setup(r => r.GetByCodeAsync("en", It.IsAny<CancellationToken>()))
            .ReturnsAsync(lang);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBe(Guid.Empty);
        _categoryRepoMock.Verify(r => r.AddAsync(It.IsAny<SkillCategory>(), It.IsAny<CancellationToken>()), Times.Once);
        _translationRepoMock.Verify(r => r.AddAsync(It.IsAny<SkillCategoryTranslation>(), It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenKeyAlreadyExists()
    {
        // Arrange
        var command = new CreateSkillCategoryCommand("frontend", 1, []);

        _categoryRepoMock.Setup(r => r.ExistsByKeyAsync(command.Key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("already exists");
        _categoryRepoMock.Verify(r => r.AddAsync(It.IsAny<SkillCategory>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenLanguageNotFound()
    {
        // Arrange
        var command = new CreateSkillCategoryCommand("devops", 2, new List<SkillCategoryTranslationDto>
        {
            new() { LanguageCode = "de", Name = "DevOps", Description = "Desc" }
        });

        _categoryRepoMock.Setup(r => r.ExistsByKeyAsync(command.Key, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _languageRepoMock.Setup(r => r.GetByCodeAsync("de", It.IsAny<CancellationToken>()))
            .ReturnsAsync((Language?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Language");
        _categoryRepoMock.Verify(r => r.AddAsync(It.IsAny<SkillCategory>(), It.IsAny<CancellationToken>()), Times.Once);
        _translationRepoMock.Verify(r => r.AddAsync(It.IsAny<SkillCategoryTranslation>(), It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenExceptionThrown()
    {
        // Arrange
        var command = new CreateSkillCategoryCommand("mobile", 3, []);
        _categoryRepoMock.Setup(r => r.ExistsByKeyAsync(command.Key, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Contain("Error occurred");
    }
}