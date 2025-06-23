using PersonalSite.Application.Features.Skills.LearningSkills.Commands.CreateLearningSkill;
using PersonalSite.Domain.Enums;

namespace PersonalSite.Application.Tests.Validators.Skills.LearningSkills;

public class CreateLearningSkillCommandValidatorTests
    {
        private readonly CreateLearningSkillCommandValidator _validator = new();

        [Fact]
        public void Should_Pass_Validation_When_Command_Is_Valid()
        {
            // Arrange
            var command = new CreateLearningSkillCommand(
                SkillId: Guid.NewGuid(),
                LearningStatus: LearningStatus.Practising,
                DisplayOrder: 1);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_When_SkillId_Is_Empty()
        {
            // Arrange
            var command = new CreateLearningSkillCommand(
                SkillId: Guid.Empty,
                LearningStatus: LearningStatus.InProgress,
                DisplayOrder: 2);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "SkillId");
        }

        [Fact]
        public void Should_Fail_When_DisplayOrder_Is_Negative()
        {
            // Arrange
            var command = new CreateLearningSkillCommand(
                SkillId: Guid.NewGuid(),
                LearningStatus: LearningStatus.Planning,
                DisplayOrder: -1);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().ContainSingle(e => e.PropertyName == "DisplayOrder");
        }

        [Fact]
        public void Should_Fail_When_SkillId_Is_Empty_And_DisplayOrder_Is_Negative()
        {
            // Arrange
            var command = new CreateLearningSkillCommand(
                SkillId: Guid.Empty,
                LearningStatus: LearningStatus.Planning,
                DisplayOrder: -10);

            // Act
            var result = _validator.Validate(command);

            // Assert
            result.IsValid.Should().BeFalse();
            result.Errors.Should().HaveCount(2);
            result.Errors.Should().Contain(e => e.PropertyName == "SkillId");
            result.Errors.Should().Contain(e => e.PropertyName == "DisplayOrder");
        }
    }