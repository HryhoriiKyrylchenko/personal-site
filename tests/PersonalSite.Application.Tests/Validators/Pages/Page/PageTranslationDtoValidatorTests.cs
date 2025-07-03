using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Pages.Page.Commands.CreatePage;
using PersonalSite.Application.Features.Pages.Page.Dtos;

namespace PersonalSite.Application.Tests.Validators.Pages.Page;

public class PageTranslationDtoValidatorTests
{
    private readonly PageTranslationDtoValidator _validator;

    public PageTranslationDtoValidatorTests()
    {
        _validator = new PageTranslationDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_LanguageCode_Is_Empty()
    {
        var dto = new PageTranslationDto { LanguageCode = "" };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.LanguageCode)
            .WithErrorMessage("LanguageCode is required.");
    }

    [Fact]
    public void Should_Have_Error_When_LanguageCode_Exceeds_MaxLength()
    {
        var dto = new PageTranslationDto { LanguageCode = new string('a', 11) };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.LanguageCode)
            .WithErrorMessage("LanguageCode must be 10 characters or fewer.");
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Empty()
    {
        var dto = new PageTranslationDto { Title = "" };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Title_Exceeds_MaxLength()
    {
        var dto = new PageTranslationDto { Title = new string('a', 256) };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title must be 255 characters or fewer.");
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Description()
    {
        var dto = new PageTranslationDto { Description = new string('a', 500) };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_MaxLength()
    {
        var dto = new PageTranslationDto { Description = new string('a', 501) };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description must be 500 characters or fewer.");
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_MetaTitle()
    {
        var dto = new PageTranslationDto { MetaTitle = new string('a', 255) };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.MetaTitle);
    }

    [Fact]
    public void Should_Have_Error_When_MetaTitle_Exceeds_MaxLength()
    {
        var dto = new PageTranslationDto { MetaTitle = new string('a', 256) };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.MetaTitle)
            .WithErrorMessage("MetaTitle must be 255 characters or fewer.");
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_MetaDescription()
    {
        var dto = new PageTranslationDto { MetaDescription = new string('a', 500) };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.MetaDescription);
    }

    [Fact]
    public void Should_Have_Error_When_MetaDescription_Exceeds_MaxLength()
    {
        var dto = new PageTranslationDto { MetaDescription = new string('a', 501) };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.MetaDescription)
            .WithErrorMessage("MetaDescription must be 500 characters or fewer.");
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_OgImage()
    {
        var dto = new PageTranslationDto { OgImage = new string('a', 255) };

        var result = _validator.TestValidate(dto);

        result.ShouldNotHaveValidationErrorFor(x => x.OgImage);
    }

    [Fact]
    public void Should_Have_Error_When_OgImage_Exceeds_MaxLength()
    {
        var dto = new PageTranslationDto { OgImage = new string('a', 256) };

        var result = _validator.TestValidate(dto);

        result.ShouldHaveValidationErrorFor(x => x.OgImage)
            .WithErrorMessage("OgImage must be 255 characters or fewer.");
    }
}