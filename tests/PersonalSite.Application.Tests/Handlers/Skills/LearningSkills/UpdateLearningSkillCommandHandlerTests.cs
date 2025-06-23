using PersonalSite.Application.Features.Skills.LearningSkills.Commands.UpdateLearningSkill;
using PersonalSite.Domain.Entities.Skills;
using PersonalSite.Domain.Enums;
using PersonalSite.Domain.Interfaces.Repositories.Skills;

namespace PersonalSite.Application.Tests.Handlers.Skills.LearningSkills;

public class UpdateLearningSkillCommandHandlerTests
    {
        private readonly Mock<ILearningSkillRepository> _repositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<ILogger<UpdateLearningSkillCommandHandler>> _loggerMock;
        private readonly UpdateLearningSkillCommandHandler _handler;

        public UpdateLearningSkillCommandHandlerTests()
        {
            _repositoryMock = new Mock<ILearningSkillRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _loggerMock = new Mock<ILogger<UpdateLearningSkillCommandHandler>>();
            _handler = new UpdateLearningSkillCommandHandler(
                _repositoryMock.Object,
                _unitOfWorkMock.Object,
                _loggerMock.Object
            );
        }

        [Fact]
        public async Task Handle_ShouldUpdateLearningSkill_WhenEntityExists()
        {
            // Arrange
            var id = Guid.NewGuid();
            var learningSkill = new LearningSkill
            {
                Id = id,
                LearningStatus = LearningStatus.Planning,
                DisplayOrder = 1
            };

            var command = new UpdateLearningSkillCommand(id, LearningStatus.Practising, 5);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(learningSkill);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeTrue();
            learningSkill.LearningStatus.Should().Be(LearningStatus.Practising);
            learningSkill.DisplayOrder.Should().Be(5);

            _repositoryMock.Verify(r => r.UpdateAsync(learningSkill, It.IsAny<CancellationToken>()), Times.Once);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Handle_ShouldReturnFailure_WhenEntityNotFound()
        {
            // Arrange
            var id = Guid.NewGuid();
            var command = new UpdateLearningSkillCommand(id, LearningStatus.InProgress, 3);

            _repositoryMock
                .Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
                .ReturnsAsync((LearningSkill?)null);

            // Act
            var result = await _handler.Handle(command, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.Should().Be("Learning skill not found.");

            _repositoryMock.Verify(r => r.UpdateAsync(It.IsAny<LearningSkill>(), It.IsAny<CancellationToken>()), Times.Never);
            _unitOfWorkMock.Verify(u => u.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }