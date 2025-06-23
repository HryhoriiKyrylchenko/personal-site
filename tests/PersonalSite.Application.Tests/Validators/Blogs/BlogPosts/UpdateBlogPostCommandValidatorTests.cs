using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Blogs.Blog.Commands.UpdateBlogPost;
using PersonalSite.Application.Features.Blogs.Blog.Dtos;

namespace PersonalSite.Application.Tests.Validators.Blogs.BlogPosts;

public class UpdateBlogPostCommandValidatorTests
{
    private readonly UpdateBlogPostCommandValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateBlogPostCommand(
            Id: Guid.Empty,
            Slug: "valid-slug",
            CoverImage: "cover.jpg",
            IsDeleted: false,
            Translations: new List<BlogPostTranslationDto>
            {
                new() { LanguageCode = "en", Title = "Title" }
            },
            Tags: new List<BlogPostTagDto>()
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id)
              .WithErrorMessage("Blog post ID is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Slug_Is_Empty_Or_Too_Long()
    {
        var commandEmptySlug = new UpdateBlogPostCommand(
            Id: Guid.NewGuid(),
            Slug: "",
            CoverImage: "cover.jpg",
            IsDeleted: false,
            Translations: new List<BlogPostTranslationDto>
            {
                new() { LanguageCode = "en", Title = "Title" }
            },
            Tags: new List<BlogPostTagDto>()
        );

        var resultEmpty = _validator.TestValidate(commandEmptySlug);
        resultEmpty.ShouldHaveValidationErrorFor(x => x.Slug)
                   .WithErrorMessage("Slug is required.");

        var commandLongSlug = new UpdateBlogPostCommand(
            Id: Guid.NewGuid(),
            Slug: new string('a', 101),
            CoverImage: "cover.jpg",
            IsDeleted: false,
            Translations: new List<BlogPostTranslationDto>
            {
                new() { LanguageCode = "en", Title = "Title" }
            },
            Tags: new List<BlogPostTagDto>()
        );

        var resultLong = _validator.TestValidate(commandLongSlug);
        resultLong.ShouldHaveValidationErrorFor(x => x.Slug)
                  .WithErrorMessage("Slug must be 100 characters or fewer.");
    }

    [Fact]
    public void Should_Have_Error_When_Translations_Is_Empty()
    {
        var command = new UpdateBlogPostCommand(
            Id: Guid.NewGuid(),
            Slug: "valid-slug",
            CoverImage: "cover.jpg",
            IsDeleted: false,
            Translations: new List<BlogPostTranslationDto>(),
            Tags: new List<BlogPostTagDto>()
        );

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Translations)
              .WithErrorMessage("At least one translation is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid_Command()
    {
        var command = new UpdateBlogPostCommand(
            Id: Guid.NewGuid(),
            Slug: "valid-slug",
            CoverImage: "cover.jpg",
            IsDeleted: false,
            Translations: new List<BlogPostTranslationDto>
            {
                new() { LanguageCode = "en", Title = "Title", Excerpt = "Excerpt", Content = "Content" }
            },
            Tags: new List<BlogPostTagDto>
            {
                new() { Id = Guid.NewGuid(), Name = "Tech" }
            }
        );

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}