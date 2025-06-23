using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.SkillCategories.Commands.UpdateSkillCategory;
using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;

namespace PersonalSite.Application.Tests.Validators.Skills.SkillCategories;

public class UpdateSkillCategoryCommandValidatorTests
{
    private readonly UpdateSkillCategoryCommandValidator _validator;

    public UpdateSkillCategoryCommandValidatorTests()
    {
        _validator = new UpdateSkillCategoryCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Id_Is_Empty()
    {
        var command = new UpdateSkillCategoryCommand(
            Guid.Empty,
            "validkey",
            0,
            new List<SkillCategoryTranslationDto> { new SkillCategoryTranslationDto { LanguageCode = "en", SkillCategoryId = Guid.NewGuid(), Name = "Name", Description = "Desc" } });

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("Id is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Key_Is_Empty()
    {
        var command = new UpdateSkillCategoryCommand(
            Guid.NewGuid(),
            string.Empty,
            0,
            new List<SkillCategoryTranslationDto> { new SkillCategoryTranslationDto { LanguageCode = "en", SkillCategoryId = Guid.NewGuid(), Name = "Name", Description = "Desc" } });

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Key)
            .WithErrorMessage("Key is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Key_Too_Long()
    {
        var longKey = new string('a', 51);
        var command = new UpdateSkillCategoryCommand(
            Guid.NewGuid(),
            longKey,
            0,
            new List<SkillCategoryTranslationDto> { new SkillCategoryTranslationDto { LanguageCode = "en", SkillCategoryId = Guid.NewGuid(), Name = "Name", Description = "Desc" } });

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Key)
            .WithErrorMessage("Key must be 50 characters or fewer.");
    }

    [Fact]
    public void Should_Have_Error_When_DisplayOrder_Is_Negative()
    {
        var command = new UpdateSkillCategoryCommand(
            Guid.NewGuid(),
            "validkey",
            -1,
            new List<SkillCategoryTranslationDto> { new SkillCategoryTranslationDto { LanguageCode = "en", SkillCategoryId = Guid.NewGuid(), Name = "Name", Description = "Desc" } });

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.DisplayOrder)
            .WithErrorMessage("DisplayOrder must be non-negative.");
    }

    [Fact]
    public void Should_Have_Error_When_Translations_Are_Empty()
    {
        var command = new UpdateSkillCategoryCommand(
            Guid.NewGuid(),
            "validkey",
            0,
            new List<SkillCategoryTranslationDto>());

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Translations)
            .WithErrorMessage("At least one translation is required.");
    }

    [Fact]
    public void Should_Have_Error_For_Invalid_Translation()
    {
        var invalidTranslation = new SkillCategoryTranslationDto
        {
            LanguageCode = "", // invalid: empty
            SkillCategoryId = Guid.Empty, // invalid
            Name = "", // invalid
            Description = new string('a', 300) // invalid: exceeds max length
        };

        var command = new UpdateSkillCategoryCommand(
            Guid.NewGuid(),
            "validkey",
            0,
            new List<SkillCategoryTranslationDto> { invalidTranslation });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor("Translations[0].LanguageCode")
            .WithErrorMessage("Language code is required.");
        result.ShouldHaveValidationErrorFor("Translations[0].SkillCategoryId")
            .WithErrorMessage("SkillCategoryId is required.");
        result.ShouldHaveValidationErrorFor("Translations[0].Name")
            .WithErrorMessage("Name is required.");
        result.ShouldHaveValidationErrorFor("Translations[0].Description")
            .WithErrorMessage("Description must be 255 characters or fewer.");
    }

    [Fact]
    public void Should_Not_Have_Error_For_Valid_Command()
    {
        var validTranslation = new SkillCategoryTranslationDto
        {
            LanguageCode = "en",
            SkillCategoryId = Guid.NewGuid(),
            Name = "Valid Name",
            Description = "Valid Description"
        };

        var command = new UpdateSkillCategoryCommand(
            Guid.NewGuid(),
            "validkey",
            0,
            new List<SkillCategoryTranslationDto> { validTranslation });

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
        result.ShouldNotHaveValidationErrorFor(c => c.Key);
        result.ShouldNotHaveValidationErrorFor(c => c.DisplayOrder);
        result.ShouldNotHaveValidationErrorFor(c => c.Translations);
    }
}