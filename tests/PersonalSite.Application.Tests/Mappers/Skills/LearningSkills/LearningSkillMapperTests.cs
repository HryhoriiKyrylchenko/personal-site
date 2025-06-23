using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Skills.LearningSkills.Mappers;
using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Enums;

namespace PersonalSite.Application.Tests.Mappers.Skills.LearningSkills;

public class LearningSkillMapperTests
{
    private readonly Mock<ITranslatableMapper<Skill, SkillDto>> _skillMapperMock;
    private readonly Mock<IAdminMapper<Skill, SkillAdminDto>> _skillAdminMapperMock;
    private readonly LearningSkillMapper _mapper;

    public LearningSkillMapperTests()
    {
        _skillMapperMock = new Mock<ITranslatableMapper<Skill, SkillDto>>();
        _skillAdminMapperMock = new Mock<IAdminMapper<Skill, SkillAdminDto>>();
        _mapper = new LearningSkillMapper(_skillMapperMock.Object, _skillAdminMapperMock.Object);
    }

    [Fact]
    public void MapToDto_ShouldReturnMappedDto()
    {
        // Arrange
        var languageCode = "en";
        var skill = new Skill { Id = Guid.NewGuid(), Key = "test-skill" };
        var learningSkill = new LearningSkill
        {
            Id = Guid.NewGuid(),
            Skill = skill,
            LearningStatus = LearningStatus.Planning,
            DisplayOrder = 2
        };

        var expectedSkillDto = new SkillDto { Id = skill.Id, Key = "test-skill" };
        _skillMapperMock.Setup(m => m.MapToDto(skill, languageCode)).Returns(expectedSkillDto);

        // Act
        var dto = _mapper.MapToDto(learningSkill, languageCode);

        // Assert
        dto.Id.Should().Be(learningSkill.Id);
        dto.LearningStatus.Should().Be("Planning");
        dto.DisplayOrder.Should().Be(2);
        dto.Skill.Should().BeEquivalentTo(expectedSkillDto);
    }

    [Fact]
    public void MapToDtoList_ShouldReturnListOfMappedDtos()
    {
        // Arrange
        var languageCode = "en";
        var skill = new Skill { Id = Guid.NewGuid(), Key = "test" };
        var learningSkills = new List<LearningSkill>
        {
            new() { Id = Guid.NewGuid(), Skill = skill, LearningStatus = LearningStatus.Practising, DisplayOrder = 1 }
        };

        _skillMapperMock
            .Setup(m => m.MapToDto(It.IsAny<Skill>(), languageCode))
            .Returns(new SkillDto { Id = skill.Id, Key = "test" });

        // Act
        var result = _mapper.MapToDtoList(learningSkills, languageCode);

        // Assert
        result.Should().HaveCount(1);
        result[0].LearningStatus.Should().Be("Practising");
    }

    [Fact]
    public void MapToAdminDto_ShouldReturnMappedAdminDto()
    {
        // Arrange
        var skill = new Skill { Id = Guid.NewGuid(), Key = "admin" };
        var learningSkill = new LearningSkill
        {
            Id = Guid.NewGuid(),
            Skill = skill,
            LearningStatus = LearningStatus.InProgress,
            DisplayOrder = 4
        };

        var expectedAdminDto = new SkillAdminDto { Id = skill.Id, Key = "admin" };
        _skillAdminMapperMock.Setup(m => m.MapToAdminDto(skill)).Returns(expectedAdminDto);

        // Act
        var dto = _mapper.MapToAdminDto(learningSkill);

        // Assert
        dto.Id.Should().Be(learningSkill.Id);
        dto.DisplayOrder.Should().Be(4);
        dto.LearningStatus.Should().Be(LearningStatus.InProgress);
        dto.Skill.Should().BeEquivalentTo(expectedAdminDto);
    }

    [Fact]
    public void MapToAdminDtoList_ShouldReturnListOfAdminDtos()
    {
        // Arrange
        var skill = new Skill { Id = Guid.NewGuid(), Key = "admin" };
        var learningSkills = new List<LearningSkill>
        {
            new() { Id = Guid.NewGuid(), Skill = skill, LearningStatus = LearningStatus.Planning, DisplayOrder = 0 }
        };

        _skillAdminMapperMock
            .Setup(m => m.MapToAdminDto(It.IsAny<Skill>()))
            .Returns(new SkillAdminDto { Id = skill.Id, Key = "admin" });

        // Act
        var result = _mapper.MapToAdminDtoList(learningSkills);

        // Assert
        result.Should().HaveCount(1);
        result[0].Skill.Key.Should().Be("admin");
    }
}