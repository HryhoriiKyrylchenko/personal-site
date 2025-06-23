using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;
using PersonalSite.Application.Features.Skills.SkillCategories.Mappers;
using PersonalSite.Domain.Entities.Common;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Application.Tests.Mappers.Skills.SkillCategories;

public class SkillCategoryMapperTests
{
    private readonly Mock<IMapper<SkillCategoryTranslation, SkillCategoryTranslationDto>> _translationMapperMock;
    private readonly SkillCategoryMapper _mapper;

    public SkillCategoryMapperTests()
    {
        _translationMapperMock = new Mock<IMapper<SkillCategoryTranslation, SkillCategoryTranslationDto>>();
        _mapper = new SkillCategoryMapper(_translationMapperMock.Object);
    }

    private SkillCategory CreateSampleSkillCategory(string langCode = "en")
    {
        var language = new Language { Code = langCode };
        var translation = new SkillCategoryTranslation
        {
            Language = language,
            Name = "Name " + langCode,
            Description = "Description " + langCode
        };

        return new SkillCategory
        {
            Id = Guid.NewGuid(),
            Key = "sample-key",
            DisplayOrder = 1,
            Translations = new List<SkillCategoryTranslation> { translation }
        };
    }

    [Fact]
    public void MapToDto_ShouldReturnCorrectDto_WhenTranslationExists()
    {
        // Arrange
        var skillCategory = CreateSampleSkillCategory();

        // Act
        var dto = _mapper.MapToDto(skillCategory, "en");

        // Assert
        dto.Id.Should().Be(skillCategory.Id);
        dto.Key.Should().Be(skillCategory.Key);
        dto.DisplayOrder.Should().Be(skillCategory.DisplayOrder);
        dto.Name.Should().Be("Name en");
        dto.Description.Should().Be("Description en");
    }

    [Fact]
    public void MapToDto_ShouldReturnEmptyStrings_WhenTranslationNotFound()
    {
        // Arrange
        var skillCategory = CreateSampleSkillCategory();

        // Act
        var dto = _mapper.MapToDto(skillCategory, "pl");

        // Assert
        dto.Name.Should().BeEmpty();
        dto.Description.Should().BeEmpty();
    }

    [Fact]
    public void MapToDtoList_ShouldMapAllEntities()
    {
        // Arrange
        var categories = new List<SkillCategory>
        {
            CreateSampleSkillCategory(),
            CreateSampleSkillCategory()
        };

        // Act
        var dtos = _mapper.MapToDtoList(categories, "en");

        // Assert
        dtos.Should().HaveCount(2);
        dtos.All(d => d.Name.StartsWith("Name")).Should().BeTrue();
    }

    [Fact]
    public void MapToAdminDto_ShouldReturnCorrectAdminDto()
    {
        // Arrange
        var skillCategory = CreateSampleSkillCategory();

        var translationDtos = new List<SkillCategoryTranslationDto>
        {
            new() { Name = "Translated Name", Description = "Translated Description", LanguageCode = "en" }
        };

        _translationMapperMock
            .Setup(m => m.MapToDtoList(skillCategory.Translations))
            .Returns(translationDtos);

        // Act
        var adminDto = _mapper.MapToAdminDto(skillCategory);

        // Assert
        adminDto.Id.Should().Be(skillCategory.Id);
        adminDto.Key.Should().Be(skillCategory.Key);
        adminDto.DisplayOrder.Should().Be(skillCategory.DisplayOrder);
        adminDto.Translations.Should().BeEquivalentTo(translationDtos);
    }

    [Fact]
    public void MapToAdminDtoList_ShouldMapAllEntities()
    {
        // Arrange
        var categories = new List<SkillCategory>
        {
            CreateSampleSkillCategory(),
            CreateSampleSkillCategory()
        };

        _translationMapperMock
            .Setup(m => m.MapToDtoList(It.IsAny<IEnumerable<SkillCategoryTranslation>>()))
            .Returns(new List<SkillCategoryTranslationDto>());

        // Act
        var adminDtos = _mapper.MapToAdminDtoList(categories);

        // Assert
        adminDtos.Should().HaveCount(2);
    }
}