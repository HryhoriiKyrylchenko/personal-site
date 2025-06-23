using PersonalSite.Application.Features.Skills.Skills.Commands.DeleteSkill;
using PersonalSite.Application.Tests.Fixtures.TestDataFactories;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Skills.Skills;

public class DeleteSkillCommandHandlerTests
{
    private readonly Mock<ISkillRepository> _skillRepoMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<ILogger<DeleteSkillCommandHandler>> _loggerMock = new();

    private readonly DeleteSkillCommandHandler _handler;

    public DeleteSkillCommandHandlerTests()
    {
        _handler = new DeleteSkillCommandHandler(
            _skillRepoMock.Object,
            _unitOfWorkMock.Object,
            _loggerMock.Object);
    }

    [Fact]
    public async Task Handle_Should_Succeed_When_Skill_Exists()
    {
        // Arrange
        var skill = SkillsTestDataFactory.CreateSkill();
        var command = new DeleteSkillCommand(skill.Id);

        _skillRepoMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(skill);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeTrue();

        skill.IsDeleted.Should().BeTrue();
        _skillRepoMock.Verify(r => r.UpdateAsync(skill, It.IsAny<CancellationToken>()), Times.Once);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_Skill_Not_Found()
    {
        // Arrange
        var command = new DeleteSkillCommand(Guid.NewGuid());

        _skillRepoMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Skill?)null);

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Skill not found.");

        _skillRepoMock.Verify(r => r.UpdateAsync(It.IsAny<Skill>(), It.IsAny<CancellationToken>()), Times.Never);
        _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
    }

    [Fact]
    public async Task Handle_Should_Fail_When_Exception_Thrown()
    {
        // Arrange
        var command = new DeleteSkillCommand(Guid.NewGuid());

        _skillRepoMock.Setup(r => r.GetByIdAsync(command.Id, It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception("DB error"));

        // Act
        var result = await _handler.Handle(command, CancellationToken.None);

        // Assert
        result.IsSuccess.Should().BeFalse();
        result.Error.Should().Be("Error while deleting skill.");
    }
}