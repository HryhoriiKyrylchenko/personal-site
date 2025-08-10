using PersonalSite.Application.Features.Skills.LearningSkills.Commands.CreateLearningSkill;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Enums;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Skills.LearningSkills;

public class CreateLearningSkillCommandHandlerTests
    {
        private readonly Mock<ILearningSkillRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly CreateLearningSkillCommandHandler _handler;

        public CreateLearningSkillCommandHandlerTests()
        {
            _repositoryMock = new Mock<ILearningSkillRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            var loggerMock = new Mock<ILogger<CreateLearningSkillCommandHandler>>();
            _handler = new CreateLearningSkillCommandHandler(
                _repositoryMock.Object,
                _unitOfWorkMock.Object,
                loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccessResult_WhenLearningSkillIsCreated()
        {
            // Arrange
            var command = new CreateLearningSkillCommand(Guid.NewGuid(), LearningStatus.InProgress, 1);
            
            _repositoryMock
                .Setup(r => r.ExistsBySkillIdAsync(command.SkillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().NotBeEmpty();

            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<LearningSkill>(), It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenLearningSkillAlreadyExists()
        {
            // Arrange
            var command = new CreateLearningSkillCommand(Guid.NewGuid(), LearningStatus.Planned, 2);

            _repositoryMock
                .Setup(r => r.ExistsBySkillIdAsync(command.SkillId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be($"Learning skill with skill id {command.SkillId} already exists.");

            _repositoryMock.Verify(r => r.AddAsync(It.IsAny<LearningSkill>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailureResult_WhenExceptionIsThrown()
        {
            // Arrange
            var command = new CreateLearningSkillCommand(Guid.NewGuid(), LearningStatus.Practicing, 5);

            _repositoryMock
                .Setup(r => r.ExistsBySkillIdAsync(command.SkillId, It.IsAny<CancellationToken>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Failed to create learning skill.");
        }
    }