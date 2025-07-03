using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Projects.Project.Commands.CreateProject;
using PersonalSite.Application.Features.Projects.Project.Dtos;

namespace PersonalSite.Application.Tests.Validators.Projects.Project;

public class CreateProjectCommandValidatorTests
{
    private readonly CreateProjectCommandValidator _validator;

    public CreateProjectCommandValidatorTests()
    {
        _validator = new CreateProjectCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Slug_Is_Empty()
    {
        var model = new CreateProjectCommand { Slug = "" };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Slug);
    }

    [Fact]
    public void Should_Have_Error_When_Slug_Too_Long()
    {
        var model = new CreateProjectCommand { Slug = new string('a', 101) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Slug);
    }

    [Fact]
    public void Should_Have_Error_When_CoverImage_Too_Long()
    {
        var model = new CreateProjectCommand { CoverImage = new string('a', 256) };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.CoverImage);
    }

    [Theory]
    [InlineData("invalid-url")]
    [InlineData("ftp://example.com")]
    public void Should_Have_Error_When_DemoUrl_Is_Invalid(string invalidUrl)
    {
        var model = new CreateProjectCommand { DemoUrl = invalidUrl };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DemoUrl);
    }

    [Theory]
    [InlineData("http://valid.com")]
    [InlineData("https://secure.com")]
    [InlineData("")]
    public void Should_Not_Have_Error_When_DemoUrl_Is_Valid_Or_Empty(string url)
    {
        var model = new CreateProjectCommand { DemoUrl = url };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.DemoUrl);
    }

    [Theory]
    [InlineData("invalid-url")]
    [InlineData("ftp://example.com")]
    public void Should_Have_Error_When_RepoUrl_Is_Invalid(string invalidUrl)
    {
        var model = new CreateProjectCommand { RepoUrl = invalidUrl };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.RepoUrl);
    }

    [Theory]
    [InlineData("http://valid.com")]
    [InlineData("https://secure.com")]
    [InlineData("")]
    public void Should_Not_Have_Error_When_RepoUrl_Is_Valid_Or_Empty(string url)
    {
        var model = new CreateProjectCommand { RepoUrl = url };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.RepoUrl);
    }

    [Fact]
    public void Should_Have_Error_When_Translations_Is_Empty()
    {
        var model = new CreateProjectCommand { Translations = new List<ProjectTranslationDto>() };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Translations);
    }

    [Fact]
    public void Should_Have_Error_When_SkillIds_Is_Empty()
    {
        var model = new CreateProjectCommand { SkillIds = new List<Guid>() };
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.SkillIds);
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Model()
    {
        var model = new CreateProjectCommand
        {
            Slug = "valid-slug",
            CoverImage = "image.jpg",
            DemoUrl = "https://demo.com",
            RepoUrl = "https://repo.com",
            Translations = new List<ProjectTranslationDto>
            {
                new ProjectTranslationDto
                {
                    LanguageCode = "en",
                    Title = "Title",
                    MetaTitle = "MetaTitle",
                    MetaDescription = "MetaDescription",
                    OgImage = "ogimage.jpg"
                }
            },
            SkillIds = new List<Guid> { Guid.NewGuid() }
        };
        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}