using PersonalSite.Application.Features.Skills.Skills.Mappers;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Application.Tests.Mappers.Skills.Skills;

public class SkillTranslationMapperTests
{
    private readonly SkillTranslationMapper _mapper = new();

    [Fact]
    public void MapToDto_Should_Map_Correctly()
    {
        // Arrange
        var language = CommonTestDataFactory.CreateLanguage();
        var skill = SkillsTestDataFactory.CreateSkill();
        var entity = SkillsTestDataFactory.CreateTranslation(skill, language);

        // Act
        var result = _mapper.MapToDto(entity);

        // Assert
        result.Id.Should().Be(entity.Id);
        result.LanguageCode.Should().Be(language.Code);
        result.SkillId.Should().Be(skill.Id);
        result.Name.Should().Be(entity.Name);
        result.Description.Should().Be(entity.Description);
    }

    [Fact]
    public void MapToDtoList_Should_Map_List_Correctly()
    {
        // Arrange
        var language = CommonTestDataFactory.CreateLanguage();
        var skill = SkillsTestDataFactory.CreateSkill();

        var entities = new List<SkillTranslation>
        {
            SkillsTestDataFactory.CreateTranslation(skill, language),
            SkillsTestDataFactory.CreateTranslation(skill, language)
        };

        // Act
        var result = _mapper.MapToDtoList(entities);

        // Assert
        result.Should().HaveCount(2);
        result[0].SkillId.Should().Be(skill.Id);
        result[0].LanguageCode.Should().Be(language.Code);
    }
}