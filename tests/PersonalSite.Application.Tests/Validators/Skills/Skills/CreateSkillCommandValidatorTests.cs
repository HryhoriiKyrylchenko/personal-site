using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.Skills.Commands.CreateSkill;
using PersonalSite.Application.Features.Skills.Skills.Dtos;

namespace PersonalSite.Application.Tests.Validators.Skills.Skills;

public class CreateSkillCommandValidatorTests
{
    private readonly CreateSkillCommandValidator _validator = new();

    private static SkillTranslationDto CreateValidTranslationDto(string lang = "en")
    {
        return new SkillTranslationDto
        {
            Id = Guid.NewGuid(),
            LanguageCode = lang,
            SkillId = Guid.NewGuid(),
            Name = "Skill Name",
            Description = "Skill description"
        };
    }

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var command = new CreateSkillCommand(
            CategoryId: Guid.NewGuid(),
            Key: "validkey",
            Translations: new List<SkillTranslationDto>
            {
                CreateValidTranslationDto()
            });

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_When_CategoryId_Is_Empty()
    {
        var command = new CreateSkillCommand(
            CategoryId: Guid.Empty,
            Key: "validkey",
            Translations: new List<SkillTranslationDto>
            {
                CreateValidTranslationDto()
            });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.CategoryId)
            .WithErrorMessage("CategoryId is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Key_Is_Empty()
    {
        var command = new CreateSkillCommand(
            CategoryId: Guid.NewGuid(),
            Key: string.Empty,
            Translations: new List<SkillTranslationDto>
            {
                CreateValidTranslationDto()
            });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Key)
            .WithErrorMessage("Key is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Key_Too_Long()
    {
        var longKey = new string('a', 51);

        var command = new CreateSkillCommand(
            CategoryId: Guid.NewGuid(),
            Key: longKey,
            Translations: new List<SkillTranslationDto>
            {
                CreateValidTranslationDto()
            });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Key)
            .WithErrorMessage("Key must be 50 characters or fewer.");
    }

    [Fact]
    public void Should_Have_Error_When_Translations_Is_Empty()
    {
        var command = new CreateSkillCommand(
            CategoryId: Guid.NewGuid(),
            Key: "validkey",
            Translations: new List<SkillTranslationDto>());

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(x => x.Translations)
            .WithErrorMessage("At least one translation is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Translation_Is_Invalid()
    {
        // Create translation with empty LanguageCode to trigger validation error
        var invalidTranslation = new SkillTranslationDto
        {
            Id = Guid.NewGuid(),
            LanguageCode = "",
            SkillId = Guid.NewGuid(),
            Name = "Valid Name",
            Description = "Valid Description"
        };

        var command = new CreateSkillCommand(
            CategoryId: Guid.NewGuid(),
            Key: "validkey",
            Translations: new List<SkillTranslationDto> { invalidTranslation });

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor("Translations[0].LanguageCode");
    }
}