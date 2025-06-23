using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.UserSkills.Commands.DeleteUserSkill;

namespace PersonalSite.Application.Tests.Validators.Skills.UserSkills;

public class DeleteUserSkillCommandValidatorTests
{
    private readonly DeleteUserSkillCommandValidator _validator;

    public DeleteUserSkillCommandValidatorTests()
    {
        _validator = new DeleteUserSkillCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new DeleteUserSkillCommand(Guid.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("UserSkill ID is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var command = new DeleteUserSkillCommand(Guid.NewGuid());
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
    }
}