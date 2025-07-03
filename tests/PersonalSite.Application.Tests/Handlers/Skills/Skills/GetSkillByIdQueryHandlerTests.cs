using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Skills.Skills.Dtos;
using PersonalSite.Application.Features.Skills.Skills.Queries.GetSkillById;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Skills.Skills;

public class GetSkillByIdQueryHandlerTests
{
    private readonly Mock<ISkillRepository> _repositoryMock;
    private readonly Mock<IAdminMapper<Skill, SkillAdminDto>> _mapperMock;
    private readonly Mock<ILogger<GetSkillByIdQueryHandler>> _loggerMock;
    private readonly GetSkillByIdQueryHandler _handler;

    public GetSkillByIdQueryHandlerTests()
    {
        _repositoryMock = new Mock<ISkillRepository>();
        _mapperMock = new Mock<IAdminMapper<Skill, SkillAdminDto>>();
        _loggerMock = new Mock<ILogger<GetSkillByIdQueryHandler>>();

        _handler = new GetSkillByIdQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsSuccess_WhenSkillFound()
    {
        // Arrange
        var skill = SkillsTestDataFactory.CreateSkillWithTranslationsAndCategory();
        var expectedDto = SkillsTestDataFactory.MapToAdminDto(skill);

        _repositoryMock.Setup(r => r.GetWithTranslationsById(skill.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(skill);

        _mapperMock.Setup(m => m.MapToAdminDto(skill)).Returns(expectedDto);

        var query = new GetSkillByIdQuery(skill.Id);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(expectedDto);
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenSkillNotFound()
    {
        // Arrange
        var skillId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetWithTranslationsById(skillId, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Skill?)null);

        var query = new GetSkillByIdQuery(skillId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Skill not found.");
    }

    [Fact]
    public async Task Handle_ReturnsFailure_WhenExceptionThrown()
    {
        // Arrange
        var skillId = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetWithTranslationsById(skillId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("Database error"));

        var query = new GetSkillByIdQuery(skillId);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error getting skill by id.");
    }
}
