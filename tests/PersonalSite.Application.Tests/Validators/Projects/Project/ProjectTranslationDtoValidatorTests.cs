using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Projects.Project.Commands.CreateProject;
using PersonalSite.Application.Features.Projects.Project.Dtos;

namespace PersonalSite.Application.Tests.Validators.Projects.Project;

public class ProjectTranslationDtoValidatorTests
{
    private readonly ProjectTranslationDtoValidator _validator;

    public ProjectTranslationDtoValidatorTests()
    {
        _validator = new ProjectTranslationDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_LanguageCode_Is_Null_Or_Empty()
    {
        var model = new ProjectTranslationDto { LanguageCode = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.LanguageCode)
            .WithErrorMessage("Language code is required");
    }

    [Fact]
    public void Should_Have_Error_When_LanguageCode_Too_Long()
    {
        var model = new ProjectTranslationDto { LanguageCode = new string('a', 11) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.LanguageCode)
            .WithErrorMessage("Language code must be 10 characters or fewer.");
    }

    [Fact]
    public void Should_Have_Error_When_Title_Is_Null_Or_Empty()
    {
        var model = new ProjectTranslationDto { Title = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Title_Too_Long()
    {
        var model = new ProjectTranslationDto { Title = new string('a', 201) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Title)
            .WithErrorMessage("Title must be 200 characters or fewer.");
    }

    [Fact]
    public void Should_Have_Error_When_MetaTitle_Too_Long()
    {
        var model = new ProjectTranslationDto { MetaTitle = new string('a', 256) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.MetaTitle)
            .WithErrorMessage("MetaTitle must be 255 characters or fewer.");
    }

    [Fact]
    public void Should_Have_Error_When_MetaDescription_Too_Long()
    {
        var model = new ProjectTranslationDto { MetaDescription = new string('a', 501) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.MetaDescription)
            .WithErrorMessage("MetaDescription must be 500 characters or fewer.");
    }

    [Fact]
    public void Should_Have_Error_When_OgImage_Too_Long()
    {
        var model = new ProjectTranslationDto { OgImage = new string('a', 256) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.OgImage)
            .WithErrorMessage("OgImage must be 255 characters or fewer.");
    }

    [Fact]
    public void Should_Not_Have_Errors_When_Valid()
    {
        var model = new ProjectTranslationDto
        {
            LanguageCode = "en",
            Title = "Valid Title",
            MetaTitle = "Valid MetaTitle",
            MetaDescription = "Valid MetaDescription",
            OgImage = "validimage.jpg"
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}