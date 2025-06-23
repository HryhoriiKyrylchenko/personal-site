using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Skills.UserSkills.Dtos;
using PersonalSite.Application.Features.Skills.UserSkills.Queries.GetUserSkillById;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Skills.UserSkills;

public class GetUserSkillByIdQueryHandlerTests
{
    private readonly Mock<IUserSkillRepository> _repositoryMock = new();
    private readonly Mock<IAdminMapper<UserSkill, UserSkillAdminDto>> _mapperMock = new();
    private readonly Mock<ILogger<GetUserSkillByIdQueryHandler>> _loggerMock = new();

    private readonly GetUserSkillByIdQueryHandler _handler;

    public GetUserSkillByIdQueryHandlerTests()
    {
        _handler = new GetUserSkillByIdQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WhenUserSkillFound()
    {
        // Arrange
        var userSkill = SkillsTestDataFactory.CreateUserSkill();
        var userSkillDto = SkillsTestDataFactory.MapUserSkillToAdminDto(userSkill);
        var query = new GetUserSkillByIdQuery(userSkill.Id);

        _repositoryMock.Setup(r => r.GetByIdAsync(userSkill.Id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync(userSkill);
        _mapperMock.Setup(m => m.MapToAdminDto(userSkill))
                   .Returns(userSkillDto);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(userSkillDto);

        _loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenUserSkillNotFound()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetUserSkillByIdQuery(id);

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                       .ReturnsAsync((UserSkill?)null);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("User skill not found.");
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenExceptionThrown()
    {
        // Arrange
        var id = Guid.NewGuid();
        var query = new GetUserSkillByIdQuery(id);
        var exception = new Exception("Some error");

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                       .ThrowsAsync(exception);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Error getting user skill by id.");
        _loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error getting user skill by id.")),
            exception,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()), Times.Once);}
}