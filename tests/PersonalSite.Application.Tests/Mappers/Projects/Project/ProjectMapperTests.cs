using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Projects.Project.Dtos;
using PersonalSite.Application.Features.Projects.Project.Mappers;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Entities.Translations;
using PersonalSite.Infrastructure.Storage;

namespace PersonalSite.Application.Tests.Mappers.Projects.Project;

public class ProjectMapperTests
{
    private readonly Mock<IS3UrlBuilder> _s3UrlBuilderMock;
    private readonly Mock<ITranslatableMapper<ProjectSkill, ProjectSkillDto>> _skillMapperMock;
    private readonly Mock<IAdminMapper<ProjectSkill, ProjectSkillAdminDto>> _skillAdminMapperMock;
    private readonly Mock<IMapper<ProjectTranslation, ProjectTranslationDto>> _translationMapperMock;
    private readonly ProjectMapper _mapper;

    public ProjectMapperTests()
    {
        _s3UrlBuilderMock = new Mock<IS3UrlBuilder>();
        _skillMapperMock = new Mock<ITranslatableMapper<ProjectSkill, ProjectSkillDto>>();
        _skillAdminMapperMock = new Mock<IAdminMapper<ProjectSkill, ProjectSkillAdminDto>>();
        _translationMapperMock = new Mock<IMapper<ProjectTranslation, ProjectTranslationDto>>();

        _mapper = new ProjectMapper(
            _s3UrlBuilderMock.Object,
            _skillMapperMock.Object,
            _skillAdminMapperMock.Object,
            _translationMapperMock.Object);
    }

    [Fact]
    public void MapToAdminDto_MapsCorrectly()
    {
        // Arrange
        var entity = ProjectTestDataFactory.CreateProject();
        var translationDtos = entity.Translations.Select(t => new ProjectTranslationDto { Id = t.Id }).ToList();
        var skillDtos = entity.ProjectSkills.Select(s => new ProjectSkillAdminDto { Id = s.Id }).ToList();

        _translationMapperMock.Setup(m => m.MapToDtoList(entity.Translations)).Returns(translationDtos);
        _skillAdminMapperMock.Setup(m => m.MapToAdminDtoList(entity.ProjectSkills)).Returns(skillDtos);

        // Act
        var result = _mapper.MapToAdminDto(entity);

        // Assert
        result.Id.Should().Be(entity.Id);
        result.Slug.Should().Be(entity.Slug);
        result.Translations.Should().BeEquivalentTo(translationDtos);
        result.Skills.Should().BeEquivalentTo(skillDtos);
    }

    [Fact]
    public void MapToDto_MapsCorrectly()
    {
        // Arrange
        var languageCode = "en";
        var entity = ProjectTestDataFactory.CreateProject();
        var translation = entity.Translations.First(t => t.Language.Code == languageCode);
        var skills = new List<ProjectSkillDto> { new ProjectSkillDto { Id = Guid.NewGuid() } };

        _s3UrlBuilderMock.Setup(b => b.BuildUrl(It.IsAny<string>())).Returns<string>(x => $"https://cdn.test/{x}");
        _skillMapperMock.Setup(m => m.MapToDtoList(entity.ProjectSkills, languageCode)).Returns(skills);

        // Act
        var result = _mapper.MapToDto(entity, languageCode);

        // Assert
        result.Id.Should().Be(entity.Id);
        result.Slug.Should().Be(entity.Slug);
        result.Title.Should().Be(translation.Title);
        result.Skills.Should().BeEquivalentTo(skills);
    }
}