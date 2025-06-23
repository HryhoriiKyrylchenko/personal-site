using PersonalSite.Application.Common.Mapping;
using PersonalSite.Application.Features.Skills.UserSkills.Dtos;
using PersonalSite.Application.Features.Skills.UserSkills.Queries.GetUserSkills;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Skills.UserSkills;

public class GetUserSkillsQueryHandlerTests
{
    private readonly Mock<IUserSkillRepository> _repositoryMock = new();
    private readonly Mock<IAdminMapper<UserSkill, UserSkillAdminDto>> _mapperMock = new();
    private readonly Mock<ILogger<GetUserSkillsQueryHandler>> _loggerMock = new();

    private readonly GetUserSkillsQueryHandler _handler;

    public GetUserSkillsQueryHandlerTests()
    {
        _handler = new GetUserSkillsQueryHandler(
            _repositoryMock.Object,
            _loggerMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccessResult_WithMappedItems()
    {
        // Arrange
        var userSkills = new List<UserSkill>
        {
            SkillsTestDataFactory.CreateUserSkill(),
            SkillsTestDataFactory.CreateUserSkill()
        };
        var mappedDtos = new List<UserSkillAdminDto>
        {
            SkillsTestDataFactory.MapUserSkillToAdminDto(userSkills[0]),
            SkillsTestDataFactory.MapUserSkillToAdminDto(userSkills[1])
        };
        var query = new GetUserSkillsQuery();

        _repositoryMock.Setup(r => r.GetFilteredAsync(
                null, null, null, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userSkills);

        _mapperMock.Setup(m => m.MapToAdminDtoList(userSkills))
            .Returns(mappedDtos);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().BeEquivalentTo(mappedDtos);
        _loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureResult_WhenExceptionThrown()
    {
        // Arrange
        var query = new GetUserSkillsQuery();
        var exception = new Exception("Test exception");

        _repositoryMock.Setup(r => r.GetFilteredAsync(
                null, null, null, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _handler.Handle(query, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Error getting user skills.");

        _loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error getting user skills.")),
            exception,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}