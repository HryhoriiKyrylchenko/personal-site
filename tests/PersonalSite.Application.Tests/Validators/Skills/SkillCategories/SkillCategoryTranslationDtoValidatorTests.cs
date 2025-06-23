using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Skills.SkillCategories.Commands.CreateSkillCategory;
using PersonalSite.Application.Features.Skills.SkillCategories.Dtos;

namespace PersonalSite.Application.Tests.Validators.Skills.SkillCategories;

public class SkillCategoryTranslationDtoValidatorTests
{
    private readonly SkillCategoryTranslationDtoValidator _validator;

    public SkillCategoryTranslationDtoValidatorTests()
    {
        _validator = new SkillCategoryTranslationDtoValidator();
    }

    [Fact]
    public void Should_Have_Error_When_LanguageCode_Is_NullOrEmpty()
    {
        var dto = new SkillCategoryTranslationDto { LanguageCode = "" };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.LanguageCode)
            .WithErrorMessage("Language code is required.");
    }

    [Theory]
    [InlineData("e")]
    [InlineData("eng")]
    [InlineData(" ")]
    public void Should_Have_Error_When_LanguageCode_Length_Is_Not_2(string langCode)
    {
        var dto = new SkillCategoryTranslationDto { LanguageCode = langCode };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.LanguageCode)
            .WithErrorMessage("Language code must be exactly 2 characters.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_LanguageCode_Is_Valid()
    {
        var dto = new SkillCategoryTranslationDto { LanguageCode = "en" };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.LanguageCode);
    }

    [Fact]
    public void Should_Have_Error_When_SkillCategoryId_Is_Empty()
    {
        var dto = new SkillCategoryTranslationDto { SkillCategoryId = Guid.Empty };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.SkillCategoryId)
            .WithErrorMessage("SkillCategoryId is required.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_SkillCategoryId_Is_Valid()
    {
        var dto = new SkillCategoryTranslationDto { SkillCategoryId = Guid.NewGuid() };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.SkillCategoryId);
    }

    [Fact]
    public void Should_Have_Error_When_Name_Is_NullOrEmpty()
    {
        var dto = new SkillCategoryTranslationDto { Name = null! };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name is required.");
    }

    [Fact]
    public void Should_Have_Error_When_Name_Exceeds_MaxLength()
    {
        var dto = new SkillCategoryTranslationDto { Name = new string('a', 101) };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name)
            .WithErrorMessage("Name must be 100 characters or fewer.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Name_Is_Valid()
    {
        var dto = new SkillCategoryTranslationDto { Name = "Valid Name" };
        var result = _validator.TestValidate(dto);
        result.ShouldNotHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public void Should_Have_Error_When_Description_Exceeds_MaxLength()
    {
        var dto = new SkillCategoryTranslationDto { Description = new string('a', 256) };
        var result = _validator.TestValidate(dto);
        result.ShouldHaveValidationErrorFor(x => x.Description)
            .WithErrorMessage("Description must be 255 characters or fewer.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Description_Is_Valid_Or_Empty()
    {
        var dto1 = new SkillCategoryTranslationDto { Description = "Valid description" };
        var result1 = _validator.TestValidate(dto1);
        result1.ShouldNotHaveValidationErrorFor(x => x.Description);

        var dto2 = new SkillCategoryTranslationDto { Description = string.Empty };
        var result2 = _validator.TestValidate(dto2);
        result2.ShouldNotHaveValidationErrorFor(x => x.Description);
    }
}