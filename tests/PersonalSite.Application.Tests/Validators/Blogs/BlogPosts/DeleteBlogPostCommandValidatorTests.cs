using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Blogs.Blog.Commands.DeleteBlogPost;

namespace PersonalSite.Application.Tests.Validators.Blogs.BlogPosts;

public class DeleteBlogPostCommandValidatorTests
{
    private readonly DeleteBlogPostCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new DeleteBlogPostCommand(Guid.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Blog post ID is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Id_Is_Valid()
    {
        var command = new DeleteBlogPostCommand(Guid.NewGuid());
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.Id);
    }
}
