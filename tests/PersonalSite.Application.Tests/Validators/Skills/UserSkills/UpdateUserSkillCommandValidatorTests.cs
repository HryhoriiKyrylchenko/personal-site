using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.UserSkills.Commands.UpdateUserSkill;

namespace PersonalSite.Application.Tests.Validators.Skills.UserSkills;

public class UpdateUserSkillCommandValidatorTests
{
    private readonly UpdateUserSkillCommandValidator _validator;

    public UpdateUserSkillCommandValidatorTests()
    {
        _validator = new UpdateUserSkillCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateUserSkillCommand(Guid.Empty, 3);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("UserSkill ID is required.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    public void Should_Have_Error_When_Proficiency_Out_Of_Range(short proficiency)
    {
        var command = new UpdateUserSkillCommand(Guid.NewGuid(), proficiency);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Proficiency)
            .WithErrorMessage("Proficiency must be between 1 and 5.");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public void Should_Not_Have_Error_When_Proficiency_In_Range(short proficiency)
    {
        var command = new UpdateUserSkillCommand(Guid.NewGuid(), proficiency);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Proficiency);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_And_Proficiency_Valid()
    {
        var command = new UpdateUserSkillCommand(Guid.NewGuid(), 3);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
        result.ShouldNotHaveValidationErrorFor(c => c.Proficiency);
    }
}