using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Skills.LearningSkills.Dtos;
using PersonalSite.Application.Features.Skills.LearningSkills.Queries.GetLearningSkills;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Enums;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Skills.LearningSkills;

public class GetLearningSkillsQueryHandlerTests
{
    private readonly Mock<ILearningSkillRepository> _repositoryMock;
    private readonly Mock<IAdminMapper<LearningSkill, LearningSkillAdminDto>> _mapperMock;
    private readonly GetLearningSkillsQueryHandler _handler;

    public GetLearningSkillsQueryHandlerTests()
    {
        _repositoryMock = new Mock<ILearningSkillRepository>();
        var loggerMock = new Mock<ILogger<GetLearningSkillsQueryHandler>>();
        _mapperMock = new Mock<IAdminMapper<LearningSkill, LearningSkillAdminDto>>();
        _handler = new GetLearningSkillsQueryHandler(
            _repositoryMock.Object,
            loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenLearningSkillsExist()
    {
        // Arrange
        var learningSkills = new List<LearningSkill>
        {
            SkillsTestDataFactory.CreateLearningSkillWithSkill()
        };
        var dtos = new List<LearningSkillAdminDto>
        {
            SkillsTestDataFactory.MapToLearningSkillAdminDto(learningSkills[0])
        };

        _repositoryMock.Setup(r => r.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(learningSkills);
        _mapperMock.Setup(m => m.MapToAdminDtoList(learningSkills))
            .Returns(dtos);

        // Act
        var result = await _handler.Handle(new GetLearningSkillsQuery(LearningStatus.InProgress), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(dtos);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenNoLearningSkillsFound()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(new List<LearningSkill>());

        // Act
        var result = await _handler.Handle(new GetLearningSkillsQuery(LearningStatus.InProgress), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Learning skills not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        _repositoryMock.Setup(r => r.ListAsync(It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        // Act
        var result = await _handler.Handle(new GetLearningSkillsQuery(LearningStatus.InProgress), CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Failed to get learning skills.");
    }
}