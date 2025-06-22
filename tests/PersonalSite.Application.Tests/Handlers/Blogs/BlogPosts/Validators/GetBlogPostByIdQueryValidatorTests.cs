using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Blogs.Blog.Queries.GetBlogPostById;

namespace PersonalSite.Application.Tests.Handlers.Blogs.BlogPosts.Validators;

public class GetBlogPostByIdQueryValidatorTests
{
    private readonly GetBlogPostByIdQueryValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var query = new GetBlogPostByIdQuery(Guid.Empty);
        var result = _validator.TestValidate(query);
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Blog post ID is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var query = new GetBlogPostByIdQuery(Guid.NewGuid());
        var result = _validator.TestValidate(query);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
