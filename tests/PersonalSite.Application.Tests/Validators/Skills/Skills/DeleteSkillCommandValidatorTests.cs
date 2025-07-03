using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.Skills.Commands.DeleteSkill;

namespace PersonalSite.Application.Tests.Validators.Skills.Skills;

public class DeleteSkillCommandValidatorTests
{
    private readonly DeleteSkillCommandValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Id_Is_Valid()
    {
        // Arrange
        var command = new DeleteSkillCommand(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_Id_Is_Empty()
    {
        // Arrange
        var command = new DeleteSkillCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("Skill ID is required.");
    }
}