using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.LearningSkills.Commands.UpdateLearningSkill;
using PersonalSite.Domain.Enums;

namespace PersonalSite.Application.Tests.Validators.Skills.LearningSkills;

public class UpdateLearningSkillCommandValidatorTests
    {
        private readonly UpdateLearningSkillCommandValidator _validator = new();

        [Fact]
        public void Should_Pass_Validation_When_All_Properties_Are_Valid()
        {
            var command = new UpdateLearningSkillCommand(
                Guid.NewGuid(),
                LearningStatus.InProgress,  // Assuming LearningStatus enum has this value
                5);

            var result = _validator.TestValidate(command);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public void Should_Fail_Validation_When_Id_Is_Empty()
        {
            var command = new UpdateLearningSkillCommand(
                Guid.Empty,
                LearningStatus.Practicing,
                0);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.Id)
                .WithErrorMessage("LearningSkill ID is required.");
        }

        [Fact]
        public void Should_Fail_Validation_When_LearningStatus_Is_Invalid()
        {
            var command = new UpdateLearningSkillCommand(
                Guid.NewGuid(),
                (LearningStatus)999, // invalid enum value
                0);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.LearningStatus)
                .WithErrorMessage("LearningStatus must be a valid enum value.");
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(-100)]
        public void Should_Fail_Validation_When_DisplayOrder_Is_Negative(short displayOrder)
        {
            var command = new UpdateLearningSkillCommand(
                Guid.NewGuid(),
                LearningStatus.Planned,
                displayOrder);

            var result = _validator.TestValidate(command);

            result.ShouldHaveValidationErrorFor(c => c.DisplayOrder)
                .WithErrorMessage("DisplayOrder cannot be negative.");
        }
    }