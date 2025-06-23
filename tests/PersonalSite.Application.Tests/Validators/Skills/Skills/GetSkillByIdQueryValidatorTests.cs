using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.Skills.Queries.GetSkillById;

namespace PersonalSite.Application.Tests.Validators.Skills.Skills;

public class GetSkillByIdQueryValidatorTests
{
    private readonly GetSkillByIdQueryValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Id_Is_Valid()
    {
        // Arrange
        var query = new GetSkillByIdQuery(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_Id_Is_Empty()
    {
        // Arrange
        var query = new GetSkillByIdQuery(Guid.Empty);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id is required.");
    }
}