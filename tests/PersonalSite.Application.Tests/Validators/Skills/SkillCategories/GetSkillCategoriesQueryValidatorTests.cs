using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.SkillCategories.Queries.GetSkillCategories;

namespace PersonalSite.Application.Tests.Validators.Skills.SkillCategories;

public class GetSkillCategoriesQueryValidatorTests
{
    private readonly GetSkillCategoriesQueryValidator _validator;

    public GetSkillCategoriesQueryValidatorTests()
    {
        _validator = new GetSkillCategoriesQueryValidator();
    }

    [Fact]
    public void Should_Have_Error_When_KeyFilter_Too_Long()
    {
        var longKey = new string('a', 51);
        var query = new GetSkillCategoriesQuery(KeyFilter: longKey);

        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.KeyFilter)
            .WithErrorMessage("Key filter must be 50 characters or fewer.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_KeyFilter_Is_Null_Or_Whitespace()
    {
        var queries = new[]
        {
            new GetSkillCategoriesQuery(KeyFilter: null),
            new GetSkillCategoriesQuery(KeyFilter: ""),
            new GetSkillCategoriesQuery(KeyFilter: "   ")
        };

        foreach (var query in queries)
        {
            var result = _validator.TestValidate(query);
            result.ShouldNotHaveValidationErrorFor(q => q.KeyFilter);
        }
    }

    [Fact]
    public void Should_Have_Error_When_MinDisplayOrder_Greater_Than_MaxDisplayOrder()
    {
        var query = new GetSkillCategoriesQuery(
            KeyFilter: null,
            MinDisplayOrder: 10,
            MaxDisplayOrder: 5);

        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q)
            .WithErrorMessage("MinDisplayOrder cannot be greater than MaxDisplayOrder.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_MinDisplayOrder_Less_Or_Equal_MaxDisplayOrder()
    {
        var queries = new[]
        {
            new GetSkillCategoriesQuery(MinDisplayOrder: null, MaxDisplayOrder: 5),
            new GetSkillCategoriesQuery(MinDisplayOrder: 5, MaxDisplayOrder: null),
            new GetSkillCategoriesQuery(MinDisplayOrder: 5, MaxDisplayOrder: 5),
            new GetSkillCategoriesQuery(MinDisplayOrder: 3, MaxDisplayOrder: 5)
        };

        foreach (var query in queries)
        {
            var result = _validator.TestValidate(query);
            result.ShouldNotHaveValidationErrorFor(q => q);
        }
    }
}