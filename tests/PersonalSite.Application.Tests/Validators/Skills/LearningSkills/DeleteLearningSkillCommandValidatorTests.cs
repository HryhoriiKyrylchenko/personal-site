using PersonalSite.Application.Features.Skills.LearningSkills.Commands.DeleteLearningSkill;

namespace PersonalSite.Application.Tests.Validators.Skills.LearningSkills;

public class DeleteLearningSkillCommandValidatorTests
{
    private readonly DeleteLearningSkillCommandValidator _validator = new();

    [Fact]
    public void Should_Pass_Validation_When_Id_Is_Valid()
    {
        // Arrange
        var command = new DeleteLearningSkillCommand(Guid.NewGuid());

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_Validation_When_Id_Is_Empty()
    {
        // Arrange
        var command = new DeleteLearningSkillCommand(Guid.Empty);

        // Act
        var result = _validator.Validate(command);

        // Assert
        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle(e => e.PropertyName == "Id");
        result.Errors[0].ErrorMessage.Should().Be("LearningSkill ID is required.");
    }
}