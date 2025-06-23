using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Common.LogEntries.Queries.GetLogEntries;

namespace PersonalSite.Application.Tests.Validators.Common.LogEntries;

public class GetLogEntriesQueryValidatorTests
{
    private readonly GetLogEntriesQueryValidator _validator;

    public GetLogEntriesQueryValidatorTests()
    {
        _validator = new GetLogEntriesQueryValidator();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Have_Error_When_Page_Is_Less_Than_1(int page)
    {
        var query = new GetLogEntriesQuery(Page: page);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.Page);
    }

    [Fact]
    public void Should_Not_Have_Error_When_Page_Is_Valid()
    {
        var query = new GetLogEntriesQuery(Page: 1);
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(q => q.Page);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void Should_Have_Error_When_PageSize_Is_Out_Of_Range(int pageSize)
    {
        var query = new GetLogEntriesQuery(PageSize: pageSize);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(q => q.PageSize);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    public void Should_Not_Have_Error_When_PageSize_Is_Within_Range(int pageSize)
    {
        var query = new GetLogEntriesQuery(PageSize: pageSize);
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(q => q.PageSize);
    }
}