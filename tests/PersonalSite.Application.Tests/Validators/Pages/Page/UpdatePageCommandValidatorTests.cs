using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Pages.Page.Commands.UpdatePage;
using PersonalSite.Application.Features.Pages.Page.Dtos;

namespace PersonalSite.Application.Tests.Validators.Pages.Page;

public class UpdatePageCommandValidatorTests
{
    private readonly UpdatePageCommandValidator _validator;

    public UpdatePageCommandValidatorTests()
    {
        _validator = new UpdatePageCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdatePageCommand(
            Guid.Empty,
            "valid-key",
            "pageimage.pgn",
            new List<PageTranslationDto> { new PageTranslationDto { LanguageCode = "en", Title = "Title" } });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("Page ID is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Key_Is_Empty()
    {
        var command = new UpdatePageCommand(
            Guid.NewGuid(),
            "",
            "pageimage.pgn",
            new List<PageTranslationDto> { new PageTranslationDto { LanguageCode = "en", Title = "Title" } });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Key)
            .WithErrorMessage("Key is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Key_Exceeds_MaxLength()
    {
        var longKey = new string('a', 51);

        var command = new UpdatePageCommand(
            Guid.NewGuid(),
            longKey,
            "pageimage.pgn",
            new List<PageTranslationDto> { new PageTranslationDto { LanguageCode = "en", Title = "Title" } });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Key)
            .WithErrorMessage("Key must be 50 characters or fewer.");
    }

    [Fact]
    public void Should_Have_Error_When_Translations_Is_Empty()
    {
        var command = new UpdatePageCommand(
            Guid.NewGuid(),
            "valid-key",
            "pageimage.pgn",
            new List<PageTranslationDto>());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Translations)
            .WithErrorMessage("At least one translation is required.");
    }

    [Fact]
    public void Should_Have_Error_For_Invalid_Translation()
    {
        var translations = new List<PageTranslationDto>
        {
            new PageTranslationDto { LanguageCode = "", Title = "" } // invalid translation
        };

        var command = new UpdatePageCommand(
            Guid.NewGuid(),
            "valid-key",
            "pageimage.pgn",
            translations);

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor("Translations[0].LanguageCode")
            .WithErrorMessage("LanguageCode is required.");
        result.ShouldHaveValidationErrorFor("Translations[0].Title")
            .WithErrorMessage("Title is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Command()
    {
        var translations = new List<PageTranslationDto>
        {
            new PageTranslationDto { LanguageCode = "en", Title = "Valid Title" }
        };

        var command = new UpdatePageCommand(
            Guid.NewGuid(),
            "valid-key",
            "pageimage.pgn",
            translations);

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Id);
        result.ShouldNotHaveValidationErrorFor(c => c.Key);
        result.ShouldNotHaveValidationErrorFor(c => c.Translations);
    }
}