using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Pages.Page.Queries.GetPageById;

namespace PersonalSite.Application.Tests.Validators.Pages.Page;

public class GetPageByIdQueryValidatorTests
{
    private readonly GetPageByIdQueryValidator _validator;

    public GetPageByIdQueryValidatorTests()
    {
        _validator = new GetPageByIdQueryValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var query = new GetPageByIdQuery(Guid.Empty);

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(q => q.Id)
            .WithErrorMessage("Id is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var query = new GetPageByIdQuery(Guid.NewGuid());

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(q => q.Id);
    }
}