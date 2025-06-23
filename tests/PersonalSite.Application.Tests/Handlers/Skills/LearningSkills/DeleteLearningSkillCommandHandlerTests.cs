using PersonalSite.Application.Features.Skills.LearningSkills.Commands.DeleteLearningSkill;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Skills.LearningSkills;

public class DeleteLearningSkillCommandHandlerTests
    {
        private readonly Mock<ILearningSkillRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<DeleteLearningSkillCommandHandler>> _loggerMock;
        private readonly DeleteLearningSkillCommandHandler _handler;

        public DeleteLearningSkillCommandHandlerTests()
        {
            _repositoryMock = new Mock<ILearningSkillRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<DeleteLearningSkillCommandHandler>>();
            _handler = new DeleteLearningSkillCommandHandler(
                _repositoryMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object);
        }

        [Fact]
        public async Task Handle_ShouldReturnSuccess_WhenLearningSkillIsSoftDeleted()
        {
            // Arrange
            var id = Guid.NewGuid();
            var learningSkill = new LearningSkill { Id = id, IsDeleted = false };

            _repositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(learningSkill);

            // Act
            var result = await _handler.Handle(new DeleteLearningSkillCommand(id), CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            learningSkill.IsDeleted.Should().BeTrue();

            _repositoryMock.Verify(r => r.UpdateAsync(learningSkill, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenLearningSkillNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();

            _repositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((LearningSkill?)null);

            // Act
            var result = await _handler.Handle(new DeleteLearningSkillCommand(id), CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Learning skill not found.");

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<LearningSkill>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }