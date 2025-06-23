using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.LearningSkills.Queries.GetLearningSkillById;

namespace PersonalSite.Application.Tests.Validators.Skills.LearningSkills;

public class GetLearningSkillByIdQueryValidatorTests
{
    private readonly GetLearningSkillByIdQueryValidator _validator = new();

    [Fact]
    public void Should_Pass_Validation_When_Id_Is_Not_Empty()
    {
        var query = new GetLearningSkillByIdQuery(Guid.NewGuid());
        var result = _validator.TestValidate(query);
        result.IsValid.Should().BeTrue();
    }

    [Fact]
    public void Should_Fail_Validation_When_Id_Is_Empty()
    {
        var query = new GetLearningSkillByIdQuery(Guid.Empty);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Id)
            .WithErrorMessage("Id is required.");
    }
}