using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.UserSkills.Commands.CreateUserSkill;

namespace PersonalSite.Application.Tests.Validators.Skills.UserSkills;

public class CreateUserSkillCommandValidatorTests
{
    private readonly CreateUserSkillCommandValidator _validator;

    public CreateUserSkillCommandValidatorTests()
    {
        _validator = new CreateUserSkillCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_SkillId_Is_Empty()
    {
        var command = new CreateUserSkillCommand(Guid.Empty, 3);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.SkillId)
            .WithErrorMessage("SkillId is required.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(6)]
    [InlineData(-1)]
    public void Should_Have_Error_When_Proficiency_Is_Out_Of_Range(short proficiency)
    {
        var command = new CreateUserSkillCommand(Guid.NewGuid(), proficiency);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Proficiency)
            .WithErrorMessage("Proficiency must be between 1 and 5.");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    public void Should_Not_Have_Error_When_Proficiency_Is_Valid(short proficiency)
    {
        var command = new CreateUserSkillCommand(Guid.NewGuid(), proficiency);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Proficiency);
    }

    [Fact]
    public void Should_Not_Have_Error_When_SkillId_Is_Valid()
    {
        var command = new CreateUserSkillCommand(Guid.NewGuid(), 3);
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.SkillId);
    }
}