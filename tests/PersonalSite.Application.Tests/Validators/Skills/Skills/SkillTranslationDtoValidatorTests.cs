using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.Skills.Commands.CreateSkill;
using PersonalSite.Application.Features.Skills.Skills.Dtos;

namespace PersonalSite.Application.Tests.Validators.Skills.Skills;

public class SkillTranslationDtoValidatorTests
{
    private readonly SkillTranslationDtoValidator _validator = new();

    [Fact]
    public void Should_Pass_When_Valid()
    {
        var dto = new SkillTranslationDto
        {
            Id = Guid.NewGuid(),
            LanguageCode = "en",
            SkillId = Guid.NewGuid(),
            Name = "C#",
            Description = "C# Language"
        };

        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Should_Have_Error_When_LanguageCode_Is_Empty()
    {
        var dto = new SkillTranslationDto
        {
            LanguageCode = "",
            SkillId = Guid.NewGuid(),
            Name = "C#"
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.LanguageCode);
    }

    [Theory]
    [InlineData("e")]
    [InlineData("eng")]
    public void Should_Have_Error_When_LanguageCode_Not_Exactly_Two_Chars(string code)
    {
        var dto = new SkillTranslationDto
        {
            LanguageCode = code,
            SkillId = Guid.NewGuid(),
            Name = "C#"
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.LanguageCode);
    }

    [Fact]
    public void Should_Have_Error_When_SkillId_Is_Empty()
    {
        var dto = new SkillTranslationDto
        {
            LanguageCode = "en",
            SkillId = Guid.Empty,
            Name = "C#"
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.SkillId);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_Empty()
    {
        var dto = new SkillTranslationDto
        {
            LanguageCode = "en",
            SkillId = Guid.NewGuid(),
            Name = ""
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Too_Long()
    {
        var dto = new SkillTranslationDto
        {
            LanguageCode = "en",
            SkillId = Guid.NewGuid(),
            Name = new string('a', 101)
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Too_Long()
    {
        var dto = new SkillTranslationDto
        {
            LanguageCode = "en",
            SkillId = Guid.NewGuid(),
            Name = "Valid Name",
            Description = new string('a', 1001)
        };

        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public void Should_Pass_When_Description_Is_Empty()
    {
        var dto = new SkillTranslationDto
        {
            LanguageCode = "en",
            SkillId = Guid.NewGuid(),
            Name = "Valid Name",
            Description = ""
        };

        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.Description);
    }
}