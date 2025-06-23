using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Contact.ContactMessages.Queries.GetContactMessageById;

namespace PersonalSite.Application.Tests.Validators.Contact.ContactMessages;

public class GetContactMessageByIdQueryValidatorTests
{
    private readonly GetContactMessageByIdQueryValidator _validator;

    public GetContactMessageByIdQueryValidatorTests()
    {
        _validator = new GetContactMessageByIdQueryValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var query = new GetContactMessageByIdQuery(Guid.Empty);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Id)
            .WithErrorMessage("Id is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Not_Empty()
    {
        var query = new GetContactMessageByIdQuery(Guid.NewGuid());
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(q => q.Id);
    }
}