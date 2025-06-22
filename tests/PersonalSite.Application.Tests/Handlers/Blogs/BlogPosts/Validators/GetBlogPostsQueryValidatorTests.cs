using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Blogs.Blog.Queries.GetBlogPosts;

namespace PersonalSite.Application.Tests.Handlers.Blogs.BlogPosts.Validators;

public class GetBlogPostsQueryValidatorTests
{
    private readonly GetBlogPostsQueryValidator _validator = new();

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Should_Have_Error_When_Page_Is_Less_Than_1(int page)
    {
        var query = new GetBlogPostsQuery(page, 10, null);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Page)
            .WithErrorMessage("Page number must be greater than 0.");
    }

    [Theory]
    [InlineData(0)]
    [InlineData(101)]
    public void Should_Have_Error_When_PageSize_Is_Out_Of_Range(int pageSize)
    {
        var query = new GetBlogPostsQuery(1, pageSize, null);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.PageSize)
            .WithErrorMessage("Page size must be between 1 and 100.");
    }

    [Fact]
    public void Should_Have_Error_When_SlugFilter_Is_Too_Long()
    {
        var longSlug = new string('a', 101);
        var query = new GetBlogPostsQuery(1, 10, longSlug);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.SlugFilter)
            .WithErrorMessage("Slug filter must be 100 characters or fewer.");
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Query()
    {
        var query = new GetBlogPostsQuery(1, 10, "valid-slug");
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveAnyValidationErrors();
    }
}