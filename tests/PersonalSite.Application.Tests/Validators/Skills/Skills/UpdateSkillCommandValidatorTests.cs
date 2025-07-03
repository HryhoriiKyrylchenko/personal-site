using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.Skills.Commands.UpdateSkill;
using PersonalSite.Application.Features.Skills.Skills.Dtos;

namespace PersonalSite.Application.Tests.Validators.Skills.Skills;

public class UpdateSkillCommandValidatorTests
{
    private readonly UpdateSkillCommandValidator _validator = new();

    private UpdateSkillCommand CreateValidCommand() => new(
        Id: Guid.NewGuid(),
        CategoryId: Guid.NewGuid(),
        Key: "valid-key",
        Translations: new List<SkillTranslationDto>
        {
            new()
            {
                Id = Guid.NewGuid(),
                SkillId = Guid.NewGuid(),
                LanguageCode = "en",
                Name = "C#",
                Description = "C# Description"
            }
        });

    [Fact]
    public void Should_Pass_With_Valid_Command()
    {
        var command = CreateValidCommand();
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Fail_When_Id_Is_Empty()
    {
        var command = CreateValidCommand() with { Id = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Skill ID is required.");
    }

    [Fact]
    public void Should_Fail_When_CategoryId_Is_Empty()
    {
        var command = CreateValidCommand() with { CategoryId = Guid.Empty };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.CategoryId)
            .WithErrorMessage("CategoryId is required.");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Fail_When_Key_Is_Null_Or_Empty(string? key)
    {
        var command = CreateValidCommand() with { Key = key! };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Key)
            .WithErrorMessage("Key is required.");
    }

    [Fact]
    public void Should_Fail_When_Key_Too_Long()
    {
        var command = CreateValidCommand() with { Key = new string('a', 51) };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Key)
            .WithErrorMessage("Key must be 50 characters or fewer.");
    }

    [Fact]
    public void Should_Fail_When_Translations_Are_Empty()
    {
        var command = CreateValidCommand() with { Translations = new List<SkillTranslationDto>() };
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Translations)
            .WithErrorMessage("At least one translation is required.");
    }

    [Fact]
    public void Should_Validate_Translations_Using_SkillTranslationDtoValidator()
    {
        var invalidTranslation = new SkillTranslationDto
        {
            Id = Guid.NewGuid(),
            SkillId = Guid.Empty,
            LanguageCode = "",
            Name = "",
            Description = new string('x', 2000)
        };

        var command = CreateValidCommand() with { Translations = [invalidTranslation] };
        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor("Translations[0].LanguageCode");
        result.ShouldHaveValidationErrorFor("Translations[0].SkillId");
        result.ShouldHaveValidationErrorFor("Translations[0].Name");
        result.ShouldHaveValidationErrorFor("Translations[0].Description");
    }
}