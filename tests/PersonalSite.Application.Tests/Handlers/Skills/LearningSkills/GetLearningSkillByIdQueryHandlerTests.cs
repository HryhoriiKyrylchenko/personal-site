using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Skills.LearningSkills.Dtos;
using PersonalSite.Application.Features.Skills.LearningSkills.Queries.GetLearningSkillById;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Skills.LearningSkills;

public class GetLearningSkillByIdQueryHandlerTests
{
    private readonly Mock<ILearningSkillRepository> _repositoryMock = new();
    private readonly Mock<ILogger<GetLearningSkillByIdQueryHandler>> _loggerMock = new();
    private readonly Mock<IAdminMapper<LearningSkill, LearningSkillAdminDto>> _mapperMock = new();

    private readonly GetLearningSkillByIdQueryHandler _handler;

    public GetLearningSkillByIdQueryHandlerTests()
    {
        _handler = new GetLearningSkillByIdQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WithDto_WhenLearningSkillFound()
    {
        // Arrange
        var learningSkill = SkillsTestDataFactory.CreateLearningSkillWithSkill();
        var dto = SkillsTestDataFactory.MapToLearningSkillAdminDto(learningSkill);
        var query = new GetLearningSkillByIdQuery(learningSkill.Id);

        _repositoryMock.Setup(r => r.GetWithFullDataByIdAsync(learningSkill.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(learningSkill);

        _mapperMock.Setup(m => m.MapToAdminDto(learningSkill))
            .Returns(dto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(dto);
        _repositoryMock.Verify(r => r.GetWithFullDataByIdAsync(learningSkill.Id, It.IsAny<CancellationToken>()), Times.Once);
        _mapperMock.Verify(m => m.MapToAdminDto(learningSkill), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenLearningSkillNotFound()
    {
        // Arrange
        var query = new GetLearningSkillByIdQuery(Guid.NewGuid());

        _repositoryMock.Setup(r => r.GetWithFullDataByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((LearningSkill?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Learning skill not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenExceptionThrown()
    {
        // Arrange
        var query = new GetLearningSkillByIdQuery(Guid.NewGuid());
        var exception = new Exception("Test exception");

        _repositoryMock.Setup(r => r.GetWithFullDataByIdAsync(It.IsAny<Guid>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Error getting learning skill by id.");
    }
}