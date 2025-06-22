using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Blogs.Blog.Commands.PublishBlogPost;

namespace PersonalSite.Application.Tests.Handlers.Blogs.BlogPosts.Validators;

public class PublishBlogPostCommandValidatorTests
{
    private readonly PublishBlogPostCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new PublishBlogPostCommand
        {
            Id = Guid.Empty,
            IsPublished = true,
            PublishDate = DateTime.UtcNow
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Blog post ID is required.");
    }

    [Fact]
    public void Should_Have_Error_When_PublishDate_Is_Null_And_IsPublished_True()
    {
        var command = new PublishBlogPostCommand
        {
            Id = Guid.NewGuid(),
            IsPublished = true,
            PublishDate = null
        };

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PublishDate)
            .WithErrorMessage("Publish date is required when publishing.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_PublishDate_Is_Set_And_IsPublished_True()
    {
        var command = new PublishBlogPostCommand
        {
            Id = Guid.NewGuid(),
            IsPublished = true,
            PublishDate = DateTime.UtcNow.AddMinutes(1)
        };

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.PublishDate);
    }

    [Fact]
    public void Should_Not_Have_Error_When_IsPublished_Is_False()
    {
        var command = new PublishBlogPostCommand
        {
            Id = Guid.NewGuid(),
            IsPublished = false,
            PublishDate = null
        };

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(x => x.PublishDate);
    }
}