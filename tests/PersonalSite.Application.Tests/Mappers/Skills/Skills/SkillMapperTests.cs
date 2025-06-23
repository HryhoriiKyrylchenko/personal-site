using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Application.Features.Skills.Skills.Mappers;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Application.Tests.Mappers.Skills.Skills;

public class SkillMapperTests
{
    private readonly Mock<ITranslatableMapper<SkillCategory, SkillCategoryDto>> _categoryMapperMock = new();
    private readonly Mock<IAdminMapper<SkillCategory, SkillCategoryAdminDto>> _categoryAdminMapperMock = new();
    private readonly Mock<IMapper<SkillTranslation, SkillTranslationDto>> _translationMapperMock = new();

    private readonly SkillMapper _mapper;

    public SkillMapperTests()
    {
        _mapper = new SkillMapper(
            _categoryMapperMock.Object,
            _categoryAdminMapperMock.Object,
            _translationMapperMock.Object
        );
    }

    [Fact]
    public void MapToDto_Should_Map_Expected_Values()
    {
        // Arrange
        var languageCode = "en";
        var skill = SkillsTestDataFactory.CreateSkill("dotnet");
        var translation = skill.Translations.First();

        var expectedCategoryDto = new SkillCategoryDto
        {
            Id = skill.Category.Id,
            Key = skill.Category.Key,
            DisplayOrder = skill.Category.DisplayOrder
        };

        _categoryMapperMock
            .Setup(x => x.MapToDto(skill.Category, languageCode))
            .Returns(expectedCategoryDto);

        // Act
        var result = _mapper.MapToDto(skill, languageCode);

        // Assert
        result.Id.Should().Be(skill.Id);
        result.Key.Should().Be(skill.Key);
        result.Name.Should().Be(translation.Name);
        result.Description.Should().Be(translation.Description);
        result.Category.Should().Be(expectedCategoryDto);
    }

    [Fact]
    public void MapToDto_Should_Fallback_To_Empty_When_Translation_Missing()
    {
        // Arrange
        var skill = SkillsTestDataFactory.CreateSkill();
        skill.Translations.Clear(); // simulate missing translation
        var languageCode = "fr";

        _categoryMapperMock
            .Setup(x => x.MapToDto(skill.Category, languageCode))
            .Returns(new SkillCategoryDto { Id = skill.Category.Id, Key = skill.Category.Key });

        // Act
        var result = _mapper.MapToDto(skill, languageCode);

        // Assert
        result.Name.Should().BeEmpty();
        result.Description.Should().BeEmpty();
    }

    [Fact]
    public void MapToAdminDto_Should_Map_Correctly()
    {
        // Arrange
        var skill = SkillsTestDataFactory.CreateSkillWithTranslationsAndCategory();

        var translationDtos = skill.Translations.Select(t => new SkillTranslationDto
        {
            Id = t.Id,
            SkillId = skill.Id,
            LanguageCode = t.Language.Code,
            Name = t.Name,
            Description = t.Description
        }).ToList();

        var categoryDto = new SkillCategoryAdminDto
        {
            Id = skill.Category.Id,
            Key = skill.Category.Key,
            DisplayOrder = skill.Category.DisplayOrder
        };

        _translationMapperMock.Setup(m => m.MapToDtoList(skill.Translations)).Returns(translationDtos);
        _categoryAdminMapperMock.Setup(m => m.MapToAdminDto(skill.Category)).Returns(categoryDto);

        // Act
        var result = _mapper.MapToAdminDto(skill);

        // Assert
        result.Id.Should().Be(skill.Id);
        result.Key.Should().Be(skill.Key);
        result.Translations.Should().BeEquivalentTo(translationDtos);
        result.Category.Should().Be(categoryDto);
    }
}