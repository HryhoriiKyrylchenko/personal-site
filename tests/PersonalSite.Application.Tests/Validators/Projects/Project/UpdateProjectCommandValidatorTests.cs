using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Projects.Project.Commands.UpdateProject;
using PersonalSite.Application.Features.Projects.Project.Dtos;

namespace PersonalSite.Application.Tests.Validators.Projects.Project;

public class UpdateProjectCommandValidatorTests
{
    private readonly UpdateProjectCommandValidator _validator;

    public UpdateProjectCommandValidatorTests()
    {
        _validator = new UpdateProjectCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var model = new UpdateProjectCommand
        {
            Id = Guid.Empty,
            Slug = "valid-slug",
            Translations = new List<ProjectTranslationDto> { new ProjectTranslationDto { LanguageCode = "en", Title = "Title" } },
            SkillIds = new List<Guid> { Guid.NewGuid() }
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Project ID is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Slug_Is_Empty()
    {
        var model = new UpdateProjectCommand
        {
            Id = Guid.NewGuid(),
            Slug = "",
            Translations = new List<ProjectTranslationDto> { new ProjectTranslationDto { LanguageCode = "en", Title = "Title" } },
            SkillIds = new List<Guid> { Guid.NewGuid() }
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Slug)
            .WithErrorMessage("Slug is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Slug_Is_Too_Long()
    {
        var model = new UpdateProjectCommand
        {
            Id = Guid.NewGuid(),
            Slug = new string('a', 101),
            Translations = new List<ProjectTranslationDto> { new ProjectTranslationDto { LanguageCode = "en", Title = "Title" } },
            SkillIds = new List<Guid> { Guid.NewGuid() }
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Slug)
            .WithErrorMessage("Slug must be 100 characters or fewer.");
    }

    [Theory]
    [InlineData("http://validurl.com")]
    [InlineData("https://validurl.com")]
    [InlineData("")]
    public void Should_Not_Have_Error_When_DemoUrl_Is_Valid(string url)
    {
        var model = new UpdateProjectCommand
        {
            Id = Guid.NewGuid(),
            Slug = "valid-slug",
            DemoUrl = url,
            Translations = new List<ProjectTranslationDto> { new ProjectTranslationDto { LanguageCode = "en", Title = "Title" } },
            SkillIds = new List<Guid> { Guid.NewGuid() }
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.DemoUrl);
    }

    [Theory]
    [InlineData("invalid-url")]
    [InlineData("htp://invalid")]
    public void Should_Have_Error_When_DemoUrl_Is_Invalid(string url)
    {
        var model = new UpdateProjectCommand
        {
            Id = Guid.NewGuid(),
            Slug = "valid-slug",
            DemoUrl = url,
            Translations = new List<ProjectTranslationDto> { new ProjectTranslationDto { LanguageCode = "en", Title = "Title" } },
            SkillIds = new List<Guid> { Guid.NewGuid() }
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.DemoUrl)
            .WithErrorMessage("DemoUrl must be a valid URL or empty.");
    }

    [Theory]
    [InlineData("http://validurl.com")]
    [InlineData("https://validurl.com")]
    [InlineData("")]
    public void Should_Not_Have_Error_When_RepoUrl_Is_Valid(string url)
    {
        var model = new UpdateProjectCommand
        {
            Id = Guid.NewGuid(),
            Slug = "valid-slug",
            RepoUrl = url,
            Translations = new List<ProjectTranslationDto> { new ProjectTranslationDto { LanguageCode = "en", Title = "Title" } },
            SkillIds = new List<Guid> { Guid.NewGuid() }
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveValidationErrorFor(x => x.RepoUrl);
    }

    [Theory]
    [InlineData("invalid-url")]
    [InlineData("htp://invalid")]
    public void Should_Have_Error_When_RepoUrl_Is_Invalid(string url)
    {
        var model = new UpdateProjectCommand
        {
            Id = Guid.NewGuid(),
            Slug = "valid-slug",
            RepoUrl = url,
            Translations = new List<ProjectTranslationDto> { new ProjectTranslationDto { LanguageCode = "en", Title = "Title" } },
            SkillIds = new List<Guid> { Guid.NewGuid() }
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.RepoUrl)
            .WithErrorMessage("RepoUrl must be a valid URL or empty.");
    }

    [Fact]
    public void Should_Have_Error_When_Translations_Is_Empty()
    {
        var model = new UpdateProjectCommand
        {
            Id = Guid.NewGuid(),
            Slug = "valid-slug",
            Translations = new List<ProjectTranslationDto>(),
            SkillIds = new List<Guid> { Guid.NewGuid() }
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Translations)
            .WithErrorMessage("At least one translation is required.");
    }

    [Fact]
    public void Should_Have_Error_When_SkillIds_Contains_Empty()
    {
        var model = new UpdateProjectCommand
        {
            Id = Guid.NewGuid(),
            Slug = "valid-slug",
            Translations = new List<ProjectTranslationDto> { new ProjectTranslationDto { LanguageCode = "en", Title = "Title" } },
            SkillIds = new List<Guid> { Guid.Empty }
        };

        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor("SkillIds[0]")
            .WithErrorMessage("At least one skill is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Model()
    {
        var model = new UpdateProjectCommand
        {
            Id = Guid.NewGuid(),
            Slug = "valid-slug",
            DemoUrl = "https://validurl.com",
            RepoUrl = "https://validrepo.com",
            Translations = new List<ProjectTranslationDto> { new ProjectTranslationDto { LanguageCode = "en", Title = "Title" } },
            SkillIds = new List<Guid> { Guid.NewGuid() }
        };

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}