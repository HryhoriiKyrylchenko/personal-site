using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Common.Language.Commands.CreateLanguage;

namespace PersonalSite.Application.Tests.Validators.Common.Language;

public class CreateLanguageCommandValidatorTests
{
    private readonly CreateLanguageCommandValidator _validator = new();

    [Theory]
    [InlineData("", false, "Code is required.")]
    [InlineData("abc", false, "Code must be 2 characters or fewer.")]
    [InlineData("en", true, null)]
    public void Validate_CodeRules(string code, bool isValid, string? expectedErrorMessage)
    {
        // Arrange
        var command = new CreateLanguageCommand(code, "Some Name");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        if (!isValid)
        {
            result.ShouldHaveValidationErrorFor(c => c.Code)
                .WithErrorMessage(expectedErrorMessage);
        }
        else
        {
            result.ShouldNotHaveValidationErrorFor(c => c.Code);
        }
    }

    [Theory]
    [InlineData("", false, "Name is required.")]
    [InlineData("English", true, null)]
    public void Validate_NameRules(string name, bool isValid, string? expectedErrorMessage)
    {
        // Arrange
        var command = new CreateLanguageCommand("en", name);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        if (!isValid)
        {
            result.ShouldHaveValidationErrorFor(c => c.Name)
                .WithErrorMessage(expectedErrorMessage);
        }
        else
        {
            result.ShouldNotHaveValidationErrorFor(c => c.Name);
        }
    }
}