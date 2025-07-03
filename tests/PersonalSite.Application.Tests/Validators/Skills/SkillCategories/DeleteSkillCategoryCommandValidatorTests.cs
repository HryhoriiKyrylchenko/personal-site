using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.SkillCategories.Commands.DeleteSkillCategory;

namespace PersonalSite.Application.Tests.Validators.Skills.SkillCategories;

public class DeleteSkillCategoryCommandValidatorTests
{
    private readonly DeleteSkillCategoryCommandValidator _validator;

    public DeleteSkillCategoryCommandValidatorTests()
    {
        _validator = new DeleteSkillCategoryCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new DeleteSkillCategoryCommand(Guid.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("Id is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Not_Empty()
    {
        var command = new DeleteSkillCategoryCommand(Guid.NewGuid());
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
    }
}