using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Projects.Project.Mappers;
using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Domain.Entities.Skills;

namespace PersonalSite.Application.Tests.Mappers.Projects.Project;

public class ProjectSkillMapperTests
{
    private readonly Mock<ITranslatableMapper<Skill, SkillDto>> _skillMapperMock = new();
    private readonly Mock<IAdminMapper<Skill, SkillAdminDto>> _skillAdminMapperMock = new();
    private readonly ProjectSkillMapper _mapper;

    public ProjectSkillMapperTests()
    {
        _mapper = new ProjectSkillMapper(_skillMapperMock.Object, _skillAdminMapperMock.Object);
    }

    [Fact]
    public void MapToDto_Should_Map_Properties_Correctly()
    {
        // Arrange
        var skill = new Skill { Id = Guid.NewGuid(), Key = "csharp" };
        var skillDto = new SkillDto { Id = skill.Id, Key = "csharp" };

        var projectSkill = new ProjectSkill
        {
            Id = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            Skill = skill
        };

        _skillMapperMock.Setup(m => m.MapToDto(skill, "en")).Returns(skillDto);

        // Act
        var result = _mapper.MapToDto(projectSkill, "en");

        // Assert
        result.Id.Should().Be(projectSkill.Id);
        result.ProjectId.Should().Be(projectSkill.ProjectId);
        result.Skill.Should().BeEquivalentTo(skillDto);
    }

    [Fact]
    public void MapToDtoList_Should_Map_All_Items()
    {
        // Arrange
        var skill = new Skill { Id = Guid.NewGuid(), Key = "csharp" };
        var skillDto = new SkillDto { Id = skill.Id, Key = "csharp" };

        var projectSkillList = new List<ProjectSkill>
        {
            new() { Id = Guid.NewGuid(), ProjectId = Guid.NewGuid(), Skill = skill },
            new() { Id = Guid.NewGuid(), ProjectId = Guid.NewGuid(), Skill = skill }
        };

        _skillMapperMock.Setup(m => m.MapToDto(It.IsAny<Skill>(), "en"))
            .Returns(skillDto);

        // Act
        var result = _mapper.MapToDtoList(projectSkillList, "en");

        // Assert
        result.Should().HaveCount(2);
        result.All(ps => ps.Skill.Key == "csharp").Should().BeTrue();
    }

    [Fact]
    public void MapToAdminDto_Should_Map_Properties_Correctly()
    {
        // Arrange
        var skill = new Skill { Id = Guid.NewGuid(), Key = "csharp" };
        var skillAdminDto = new SkillAdminDto { Id = skill.Id, Key = "csharp" };

        var projectSkill = new ProjectSkill
        {
            Id = Guid.NewGuid(),
            ProjectId = Guid.NewGuid(),
            Skill = skill
        };

        _skillAdminMapperMock.Setup(m => m.MapToAdminDto(skill)).Returns(skillAdminDto);

        // Act
        var result = _mapper.MapToAdminDto(projectSkill);

        // Assert
        result.Id.Should().Be(projectSkill.Id);
        result.ProjectId.Should().Be(projectSkill.ProjectId);
        result.Skill.Should().BeEquivalentTo(skillAdminDto);
    }

    [Fact]
    public void MapToAdminDtoList_Should_Map_All_Items()
    {
        // Arrange
        var skill = new Skill { Id = Guid.NewGuid(), Key = "csharp" };
        var skillAdminDto = new SkillAdminDto { Id = skill.Id, Key = "csharp" };

        var projectSkills = new List<ProjectSkill>
        {
            new() { Id = Guid.NewGuid(), ProjectId = Guid.NewGuid(), Skill = skill },
            new() { Id = Guid.NewGuid(), ProjectId = Guid.NewGuid(), Skill = skill }
        };

        _skillAdminMapperMock.Setup(m => m.MapToAdminDto(It.IsAny<Skill>())).Returns(skillAdminDto);

        // Act
        var result = _mapper.MapToAdminDtoList(projectSkills);

        // Assert
        result.Should().HaveCount(2);
        result.All(p => p.Skill.Key == "csharp").Should().BeTrue();
    }
}