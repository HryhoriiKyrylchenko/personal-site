using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.SkillCategories.Queries.GetSkillCategoryById;

namespace PersonalSite.Application.Tests.Validators.Skills.SkillCategories;

public class GetSkillCategoryByIdQueryValidatorTests
{
    private readonly GetSkillCategoryByIdQueryValidator _validator;

    public GetSkillCategoryByIdQueryValidatorTests()
    {
        _validator = new GetSkillCategoryByIdQueryValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var query = new GetSkillCategoryByIdQuery(Guid.Empty);

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.Id)
            .WithErrorMessage("Id is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Not_Empty()
    {
        var query = new GetSkillCategoryByIdQuery(Guid.NewGuid());

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(q => q.Id);
    }
}