using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.UserSkills.Queries.GetUserSkillById;

namespace PersonalSite.Application.Tests.Validators.Skills.UserSkills;

public class GetUserSkillByIdQueryValidatorTests
{
    private readonly GetUserSkillByIdQueryValidator _validator;

    public GetUserSkillByIdQueryValidatorTests()
    {
        _validator = new GetUserSkillByIdQueryValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var query = new GetUserSkillByIdQuery(Guid.Empty);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Id)
            .WithErrorMessage("UserSkill ID is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var query = new GetUserSkillByIdQuery(Guid.NewGuid());
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(q => q.Id);
    }
}