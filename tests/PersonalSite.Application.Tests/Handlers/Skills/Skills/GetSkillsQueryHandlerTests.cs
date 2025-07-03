using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Application.Features.Skills.Skills.Queries.GetSkills;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Skills.Skills;

public class GetSkillsQueryHandlerTests
{
    private readonly Mock<ISkillRepository> _repositoryMock;
    private readonly Mock<IAdminMapper<Skill, SkillAdminDto>> _mapperMock;
    private readonly Mock<ILogger<GetSkillsQueryHandler>> _loggerMock;
    private readonly GetSkillsQueryHandler _handler;

    public GetSkillsQueryHandlerTests()
    {
        _repositoryMock = new Mock<ISkillRepository>();
        _mapperMock = new Mock<IAdminMapper<Skill, SkillAdminDto>>();
        _loggerMock = new Mock<ILogger<GetSkillsQueryHandler>>();

        _handler = new GetSkillsQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WithMappedDtos()
    {
        // Arrange
        var skill1 = SkillsTestDataFactory.CreateSkillWithTranslationsAndCategory();
        var skill2 = SkillsTestDataFactory.CreateSkillWithTranslationsAndCategory();
        var skills = new List<Skill> { skill1, skill2 };

        var expectedDtos = new List<SkillAdminDto>
        {
            SkillsTestDataFactory.MapToAdminDto(skill1),
            SkillsTestDataFactory.MapToAdminDto(skill2)
        };

        _repositoryMock.Setup(r => r.GetFilteredAsync(null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(skills);

        _mapperMock.Setup(m => m.MapToAdminDtoList(skills)).Returns(expectedDtos);

        var query = new GetSkillsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedDtos);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenExceptionThrown()
    {
        // Arrange
        _repositoryMock.Setup(r => r.GetFilteredAsync(null, null, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        var query = new GetSkillsQuery();

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error while retrieving skills with filters.");
    }
    
    [Fact]
    public async Task Handle_CallsRepositoryWithFilters()
    {
        // Arrange
        var categoryId = Guid.NewGuid();
        var keyFilter = "backend";

        _repositoryMock.Setup(r => r.GetFilteredAsync(categoryId, keyFilter, It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<Skill>());

        _mapperMock.Setup(m => m.MapToAdminDtoList(It.IsAny<IEnumerable<Skill>>()))
            .Returns(new List<SkillAdminDto>());

        var query = new GetSkillsQuery(categoryId, keyFilter);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        _repositoryMock.Verify(r => r.GetFilteredAsync(categoryId, keyFilter, It.IsAny<CancellationToken>()), Times.Once);
    }
}