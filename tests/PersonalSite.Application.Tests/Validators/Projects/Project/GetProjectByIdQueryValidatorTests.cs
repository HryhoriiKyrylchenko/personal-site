using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Projects.Project.Queries.GetProjectById;

namespace PersonalSite.Application.Tests.Validators.Projects.Project;

public class GetProjectByIdQueryValidatorTests
{
    private readonly GetProjectByIdQueryValidator _validator;

    public GetProjectByIdQueryValidatorTests()
    {
        _validator = new GetProjectByIdQueryValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var query = new GetProjectByIdQuery(Guid.Empty);

        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Id is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var query = new GetProjectByIdQuery(Guid.NewGuid());

        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}