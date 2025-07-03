using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Application.Features.Skills.UserSkills.Dtos;
using PersonalSite.Application.Features.Skills.UserSkills.Mappers;
using PersonalSite.Domain.Entities.Skills;

namespace PersonalSite.Application.Tests.Mappers.Skills.UserSkills;

public class UserSkillMapperTests
{
    private readonly Mock<ITranslatableMapper<Skill, SkillDto>> _skillMapperMock;
    private readonly Mock<IAdminMapper<Skill, SkillAdminDto>> _skillAdminMapperMock;
    private readonly UserSkillMapper _mapper;

    public UserSkillMapperTests()
    {
        _skillMapperMock = new Mock<ITranslatableMapper<Skill, SkillDto>>();
        _skillAdminMapperMock = new Mock<IAdminMapper<Skill, SkillAdminDto>>();

        _mapper = new UserSkillMapper(_skillMapperMock.Object, _skillAdminMapperMock.Object);
    }

    [Fact]
    public void MapToDto_ShouldMapCorrectly()
    {
        // Arrange
        var languageCode = "en";
        var skill = new Skill { /* init if needed */ };
        var userSkill = new UserSkill
        {
            Id = Guid.NewGuid(),
            Skill = skill,
            Proficiency = 4
        };
        var expectedSkillDto = new SkillDto { /* init if needed */ };

        _skillMapperMock
            .Setup(m => m.MapToDto(skill, languageCode))
            .Returns(expectedSkillDto);

        // Act
        var dto = _mapper.MapToDto(userSkill, languageCode);

        // Assert
        Assert.Equal(userSkill.Id, dto.Id);
        Assert.Equal(expectedSkillDto, dto.Skill);
        Assert.Equal(userSkill.Proficiency, dto.Proficiency);
    }

    [Fact]
    public void MapToDtoList_ShouldMapAll()
    {
        // Arrange
        var languageCode = "en";
        var skill = new Skill();
        var userSkills = new List<UserSkill>
        {
            new UserSkill { Id = Guid.NewGuid(), Skill = skill, Proficiency = 1 },
            new UserSkill { Id = Guid.NewGuid(), Skill = skill, Proficiency = 3 }
        };

        _skillMapperMock
            .Setup(m => m.MapToDto(It.IsAny<Skill>(), languageCode))
            .Returns(new SkillDto());

        // Act
        var dtos = _mapper.MapToDtoList(userSkills, languageCode);

        // Assert
        Assert.Equal(userSkills.Count, dtos.Count);
        foreach (var dto in dtos)
        {
            Assert.IsType<UserSkillDto>(dto);
            Assert.NotNull(dto.Skill);
        }
    }

    [Fact]
    public void MapToAdminDto_ShouldMapCorrectly()
    {
        // Arrange
        var skill = new Skill();
        var userSkill = new UserSkill
        {
            Id = Guid.NewGuid(),
            Skill = skill,
            Proficiency = 5,
            CreatedAt = DateTime.UtcNow.AddDays(-1),
            UpdatedAt = DateTime.UtcNow
        };

        var expectedAdminDto = new SkillAdminDto();

        _skillAdminMapperMock
            .Setup(m => m.MapToAdminDto(skill))
            .Returns(expectedAdminDto);

        // Act
        var adminDto = _mapper.MapToAdminDto(userSkill);

        // Assert
        Assert.Equal(userSkill.Id, adminDto.Id);
        Assert.Equal(expectedAdminDto, adminDto.Skill);
        Assert.Equal(userSkill.Proficiency, adminDto.Proficiency);
        Assert.Equal(userSkill.CreatedAt, adminDto.CreatedAt);
        Assert.Equal(userSkill.UpdatedAt, adminDto.UpdatedAt);
    }

    [Fact]
    public void MapToAdminDtoList_ShouldMapAll()
    {
        // Arrange
        var skill = new Skill();
        var userSkills = new List<UserSkill>
        {
            new UserSkill { Id = Guid.NewGuid(), Skill = skill, Proficiency = 2, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow },
            new UserSkill { Id = Guid.NewGuid(), Skill = skill, Proficiency = 3, CreatedAt = DateTime.UtcNow, UpdatedAt = DateTime.UtcNow }
        };

        _skillAdminMapperMock
            .Setup(m => m.MapToAdminDto(It.IsAny<Skill>()))
            .Returns(new SkillAdminDto());

        // Act
        var adminDtos = _mapper.MapToAdminDtoList(userSkills);

        // Assert
        Assert.Equal(userSkills.Count, adminDtos.Count);
        foreach (var dto in adminDtos)
        {
            Assert.IsType<UserSkillAdminDto>(dto);
            Assert.NotNull(dto.Skill);
        }
    }
}