using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Common.Resume.Queries.GetResumeById;

namespace PersonalSite.Application.Tests.Validators.Common.Resume;

public class GetResumeByIdQueryValidatorTests
{
    private readonly GetResumeByIdQueryValidator _validator;

    public GetResumeByIdQueryValidatorTests()
    {
        _validator = new GetResumeByIdQueryValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var query = new GetResumeByIdQuery(Guid.Empty);

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Resume ID is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var query = new GetResumeByIdQuery(Guid.NewGuid());

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}