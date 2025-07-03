using PersonalSite.Application.Features.Skills.UserSkills.Commands.UpdateUserSkill;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Skills.UserSkills;

public class UpdateUserSkillCommandHandlerTests
{
    private readonly Mock<IUserSkillRepository> _repositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<ILogger<UpdateUserSkillCommandHandler>> _loggerMock;
    private readonly UpdateUserSkillCommandHandler _handler;

    public UpdateUserSkillCommandHandlerTests()
    {
        _repositoryMock = new Mock<IUserSkillRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _loggerMock = new Mock<ILogger<UpdateUserSkillCommandHandler>>();

        _handler = new UpdateUserSkillCommandHandler(
            _repositoryMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_ShouldReturnSuccess_WhenUpdateSucceeds()
    {
        // Arrange
        var existingEntity = SkillsTestDataFactory.CreateUserSkill();
        var command = new UpdateUserSkillCommand(existingEntity.Id, 3);

        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(existingEntity);
        _repositoryMock.Setup(r => r.UpdateAsync(existingEntity, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        _unitOfWorkMock.Setup(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        _repositoryMock.Verify(r => r.UpdateAsync(existingEntity, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenEntityNotFound()
    {
        // Arrange
        var command = new UpdateUserSkillCommand(Guid.NewGuid(), 2);

        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((UserSkill?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("User skill not found.", result.Error);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailure_WhenExceptionThrown()
    {
        // Arrange
        var command = new UpdateUserSkillCommand(Guid.NewGuid(), 2);
        var exception = new Exception("Database error");

        _repositoryMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(exception);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to update user skill.", result.Error);
    }
}