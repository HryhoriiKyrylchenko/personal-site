using PersonalSite.Application.Features.Skills.UserSkills.Commands.CreateUserSkill;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Skills.UserSkills;

public class CreateUserSkillCommandHandlerTests
{
    private readonly Mock<IUserSkillRepository> _repositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<CreateUserSkillCommandHandler>> _loggerMock = new();

    private readonly CreateUserSkillCommandHandler _handler;

    public CreateUserSkillCommandHandlerTests()
    {
        _handler = new CreateUserSkillCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldCreateUserSkill_WhenSkillDoesNotExist()
    {
        // Arrange
        var skillId = Guid.NewGuid();
        var command = new CreateUserSkillCommand(skillId, 3);

        _repositoryMock.Setup(r => r.ExistsBySkillIdAsync(skillId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<UserSkill>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);


        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().NotBeEmpty();

        _repositoryMock.Verify(r => r.ExistsBySkillIdAsync(skillId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.AddAsync(It.Is<UserSkill>(u =>
            u.SkillId == skillId &&
            u.Proficiency == command.Proficiency &&
            u.Id != Guid.Empty), It.IsAny<CancellationToken>()), Times.Once);

        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        _loggerMock.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenSkillAlreadyExists()
    {
        // Arrange
        var skillId = Guid.NewGuid();
        var command = new CreateUserSkillCommand(skillId, 3);

        _repositoryMock.Setup(r => r.ExistsBySkillIdAsync(skillId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("User skill with skill Id already exists.");

        _repositoryMock.Verify(r => r.ExistsBySkillIdAsync(skillId, It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<UserSkill>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailureAndLogError_WhenExceptionThrown()
    {
        // Arrange
        var skillId = Guid.NewGuid();
        var command = new CreateUserSkillCommand(skillId, 3);
        var exception = new Exception("Database error");

        _repositoryMock.Setup(r => r.ExistsBySkillIdAsync(skillId, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsFailure.Should().BeTrue();
        result.Error.Should().Be("Failed to create user skill.");

        _loggerMock.Verify(l => l.Log(
            LogLevel.Error,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((v, t) => v.ToString()!.Contains("Error creating user skill.")),
            exception,
            It.IsAny<Func<It.IsAnyType, Exception?, string>>()),
            Times.Once);
    }
}