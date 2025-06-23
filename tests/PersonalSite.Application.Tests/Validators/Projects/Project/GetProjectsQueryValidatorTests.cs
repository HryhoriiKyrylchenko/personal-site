using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Projects.Project.Queries.GetProjects;

namespace PersonalSite.Application.Tests.Validators.Projects.Project;

public class GetProjectsQueryValidatorTests
{
    private readonly GetProjectsQueryValidator _validator;

    public GetProjectsQueryValidatorTests()
    {
        _validator = new GetProjectsQueryValidator();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Have_Error_When_Page_Is_Less_Than_1(int page)
    {
        var query = new GetProjectsQuery(Page: page);

        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Page)
            .WithErrorMessage("Page must be greater than 0.");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(100)]
    public void Should_Not_Have_Error_When_Page_Is_Valid(int page)
    {
        var query = new GetProjectsQuery(Page: page);

        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.Page);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    [InlineData(1000)]
    public void Should_Have_Error_When_PageSize_Is_Out_Of_Range(int pageSize)
    {
        var query = new GetProjectsQuery(PageSize: pageSize);

        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.PageSize)
            .WithErrorMessage("PageSize must be between 1 and 100.");
    }

    [Theory]
    [InlineData(1)]
    [InlineData(50)]
    [InlineData(100)]
    public void Should_Not_Have_Error_When_PageSize_Is_Valid(int pageSize)
    {
        var query = new GetProjectsQuery(PageSize: pageSize);

        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void Should_Have_Error_When_SlugFilter_Too_Long()
    {
        var longSlug = new string('a', 101);
        var query = new GetProjectsQuery(SlugFilter: longSlug);

        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.SlugFilter)
            .WithErrorMessage("SlugFilter must be 100 characters or fewer.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("valid-slug-filter")]
    public void Should_Not_Have_Error_When_SlugFilter_Is_Valid(string slugFilter)
    {
        var query = new GetProjectsQuery(SlugFilter: slugFilter);

        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.SlugFilter);
    }
}