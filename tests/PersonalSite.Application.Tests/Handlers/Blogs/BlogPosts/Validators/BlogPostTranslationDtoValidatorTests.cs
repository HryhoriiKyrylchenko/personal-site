using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Blogs.Blog.Commands.CreateBlogPost;
using PersonalSite.Application.Features.Blogs.Blog.Dtos;

namespace PersonalSite.Application.Tests.Handlers.Blogs.BlogPosts.Validators;

public class BlogPostTranslationDtoValidatorTests
{
    private readonly BlogPostTranslationDtoValidator _validator = new();

    [Fact]
    public void Should_Have_Error_When_LanguageCode_Is_Empty()
    {
        var dto = new BlogPostTranslationDto { LanguageCode = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.LanguageCode)
            .WithErrorMessage("Language code is required.");
    }

    [Fact]
    public void Should_Have_Error_When_LanguageCode_Length_Is_Not_2()
    {
        var dto = new BlogPostTranslationDto { LanguageCode = "eng" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.LanguageCode)
            .WithErrorMessage("Language code must be 2 characters.");
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var dto = new BlogPostTranslationDto { Title = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid_Dto()
    {
        var dto = new BlogPostTranslationDto
        {
            LanguageCode = "en",
            Title = "Valid title",
            Excerpt = "Excerpt",
            Content = "Content",
            MetaTitle = new string('a', 255),
            MetaDescription = new string('b', 500),
            OgImage = new string('c', 255)
        };

        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }
}
