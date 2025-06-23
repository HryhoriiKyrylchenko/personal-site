using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Contact.ContactMessages.Queries.GetContactMessages;

namespace PersonalSite.Application.Tests.Validators.Contact.ContactMessages;

public class GetContactMessagesQueryValidatorTests
{
    private readonly GetContactMessagesQueryValidator _validator;

    public GetContactMessagesQueryValidatorTests()
    {
        _validator = new GetContactMessagesQueryValidator();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Have_Error_When_Page_Is_Less_Than_1(int invalidPage)
    {
        var query = new GetContactMessagesQuery(Page: invalidPage, PageSize: 10);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Page)
            .WithErrorMessage("Page number must be greater than zero.");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(5)]
    [InlineData(100)]
    public void Should_Not_Have_Error_When_Page_Is_Valid(int validPage)
    {
        var query = new GetContactMessagesQuery(Page: validPage, PageSize: 10);
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(q => q.Page);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    [InlineData(101)]
    [InlineData(200)]
    public void Should_Have_Error_When_PageSize_Is_Out_Of_Range(int invalidPageSize)
    {
        var query = new GetContactMessagesQuery(Page: 1, PageSize: invalidPageSize);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.PageSize)
            .WithErrorMessage("Page size must be between 1 and 100.");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(100)]
    public void Should_Not_Have_Error_When_PageSize_Is_Valid(int validPageSize)
    {
        var query = new GetContactMessagesQuery(Page: 1, PageSize: validPageSize);
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(q => q.PageSize);
    }
}