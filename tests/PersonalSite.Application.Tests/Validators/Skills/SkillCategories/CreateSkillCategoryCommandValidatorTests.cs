using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.SkillCategories.Commands.CreateSkillCategory;
using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;

namespace PersonalSite.Application.Tests.Validators.Skills.SkillCategories;

public class CreateSkillCategoryCommandValidatorTests
{
    private readonly CreateSkillCategoryCommandValidator _validator;

    public CreateSkillCategoryCommandValidatorTests()
    {
        _validator = new CreateSkillCategoryCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Key_Is_NullOrEmpty()
    {
        var command = new CreateSkillCategoryCommand("", 0, new List<SkillCategoryTranslationDto> { CreateValidTranslation() });
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Key)
            .WithErrorMessage("Key is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Key_Exceeds_MaxLength()
    {
        var longKey = new string('a', 51);
        var command = new CreateSkillCategoryCommand(longKey, 0, new List<SkillCategoryTranslationDto> { CreateValidTranslation() });
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Key)
            .WithErrorMessage("Key must be 50 characters or fewer.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Key_Is_Valid()
    {
        var command = new CreateSkillCategoryCommand("validkey", 0, new List<SkillCategoryTranslationDto> { CreateValidTranslation() });
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Key);
    }

    [Fact]
    public void Should_Have_Error_When_DisplayOrder_Is_Negative()
    {
        var command = new CreateSkillCategoryCommand("validkey", -1, new List<SkillCategoryTranslationDto> { CreateValidTranslation() });
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.DisplayOrder)
            .WithErrorMessage("DisplayOrder must be non-negative.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_DisplayOrder_Is_NonNegative()
    {
        var command = new CreateSkillCategoryCommand("validkey", 0, new List<SkillCategoryTranslationDto> { CreateValidTranslation() });
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.DisplayOrder);
    }

    [Fact]
    public void Should_Have_Error_When_Translations_Is_Empty()
    {
        var command = new CreateSkillCategoryCommand("validkey", 0, new List<SkillCategoryTranslationDto>());
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(c => c.Translations)
            .WithErrorMessage("At least one translation is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Translations_Contain_Invalid_Translation()
    {
        var invalidTranslation = new SkillCategoryTranslationDto { LanguageCode = "", SkillCategoryId = default, Name = "", Description = new string('a', 300) };
        var command = new CreateSkillCategoryCommand("validkey", 0, new List<SkillCategoryTranslationDto> { invalidTranslation });
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor("Translations[0].LanguageCode");
        result.ShouldHaveValidationErrorFor("Translations[0].SkillCategoryId");
        result.ShouldHaveValidationErrorFor("Translations[0].Name");
        result.ShouldHaveValidationErrorFor("Translations[0].Description");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Translations_Are_Valid()
    {
        var command = new CreateSkillCategoryCommand("validkey", 0, new List<SkillCategoryTranslationDto> { CreateValidTranslation() });
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveValidationErrorFor(c => c.Translations);
    }

    private static SkillCategoryTranslationDto CreateValidTranslation()
    {
        return new SkillCategoryTranslationDto
        {
            LanguageCode = "en",
            SkillCategoryId = Guid.NewGuid(),
            Name = "Backend",
            Description = "Backend development"
        };
    }
}