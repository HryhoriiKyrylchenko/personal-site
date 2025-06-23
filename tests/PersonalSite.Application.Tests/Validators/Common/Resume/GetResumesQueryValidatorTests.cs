using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Common.Resume.Queries.GetResumes;

namespace PersonalSite.Application.Tests.Validators.Common.Resume;

public class GetResumesQueryValidatorTests
{
    private readonly GetResumesQueryValidator _validator;

    public GetResumesQueryValidatorTests()
    {
        _validator = new GetResumesQueryValidator();
    }

    [Theory]
    [InlineData(0)]    // invalid page number
    [InlineData(-1)]   // invalid page number
    public void Should_Have_Error_When_Page_Is_Less_Than_1(int page)
    {
        var query = new GetResumesQuery(page);

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.Page);
    }

    [Theory]
    [InlineData(1)]    // valid page number
    [InlineData(10)]
    public void Should_Not_Have_Error_When_Page_Is_Valid(int page)
    {
        var query = new GetResumesQuery(page);

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(x => x.Page);
    }

    [Theory]
    [InlineData(0)]    // invalid page size
    [InlineData(101)]  // invalid page size
    public void Should_Have_Error_When_PageSize_Is_Out_Of_Range(int pageSize)
    {
        var query = new GetResumesQuery(1, pageSize);

        var result = _validator.TestValidate(query);

        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Theory]
    [InlineData(1)]    // valid page size
    [InlineData(50)]
    [InlineData(100)]
    public void Should_Not_Have_Error_When_PageSize_Is_Valid(int pageSize)
    {
        var query = new GetResumesQuery(1, pageSize);

        var result = _validator.TestValidate(query);

        result.ShouldNotHaveValidationErrorFor(x => x.PageSize);
    }
}