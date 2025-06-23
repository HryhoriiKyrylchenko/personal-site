using PersonalSite.Application.Features.Skills.SkillCategories.Mappers;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Application.Tests.Mappers.Skills.SkillCategories;

public class SkillCategoryTranslationMapperTests
{
    private readonly SkillCategoryTranslationMapper _mapper = new();

    [Fact]
    public void MapToDto_ShouldMapCorrectly()
    {
        // Arrange
        var language = CommonTestDataFactory.CreateLanguage();
        var translation = new SkillCategoryTranslation
        {
            Id = Guid.NewGuid(),
            LanguageId = language.Id,
            Language = language,
            SkillCategoryId = Guid.NewGuid(),
            Name = "Backend",
            Description = "Backend development"
        };

        // Act
        var result = _mapper.MapToDto(translation);

        // Assert
        result.Id.Should().Be(translation.Id);
        result.LanguageCode.Should().Be("en");
        result.SkillCategoryId.Should().Be(translation.SkillCategoryId);
        result.Name.Should().Be("Backend");
        result.Description.Should().Be("Backend development");
    }

    [Fact]
    public void MapToDtoList_ShouldMapAllItemsCorrectly()
    {
        // Arrange
        var language = CommonTestDataFactory.CreateLanguage();
        var translations = new List<SkillCategoryTranslation>
        {
            new()
            {
                Id = Guid.NewGuid(),
                LanguageId = language.Id,
                Language = language,
                SkillCategoryId = Guid.NewGuid(),
                Name = "Backend",
                Description = "Backend dev"
            },
            new()
            {
                Id = Guid.NewGuid(),
                LanguageId = language.Id,
                Language = language,
                SkillCategoryId = Guid.NewGuid(),
                Name = "Frontend",
                Description = "Frontend dev"
            }
        };

        // Act
        var results = _mapper.MapToDtoList(translations);

        // Assert
        results.Should().HaveCount(2);
        results[0].Name.Should().Be("Backend");
        results[1].Name.Should().Be("Frontend");
    }
}