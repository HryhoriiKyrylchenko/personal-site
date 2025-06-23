using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Pages.Page.Commands.CreatePage;
using PersonalSite.Application.Features.Pages.Page.Dtos;

namespace PersonalSite.Application.Tests.Validators.Pages.Page;

public class CreatePageCommandValidatorTests
{
    private readonly CreatePageCommandValidator _validator;

    public CreatePageCommandValidatorTests()
    {
        _validator = new CreatePageCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Key_Is_Empty()
    {
        var command = new CreatePageCommand("", new List<PageTranslationDto> { new PageTranslationDto() });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Key)
            .WithErrorMessage("Key is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Key_Exceeds_MaxLength()
    {
        var longKey = new string('a', 51);
        var command = new CreatePageCommand(longKey, new List<PageTranslationDto> { new PageTranslationDto() });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Key)
            .WithErrorMessage("Key must be 50 characters or fewer.");
    }

    [Fact]
    public void Should_Have_Error_When_Translations_Is_Empty()
    {
        var command = new CreatePageCommand("valid-key", new List<PageTranslationDto>());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Translations)
            .WithErrorMessage("At least one translation is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Valid_Command()
    {
        var translations = new List<PageTranslationDto>
        {
            new PageTranslationDto { LanguageCode = "en", Title = "Title" }
        };

        var command = new CreatePageCommand("valid-key", translations);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }
}