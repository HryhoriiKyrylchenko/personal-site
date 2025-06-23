using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Common.Language.Commands.UpdateLanguage;

namespace PersonalSite.Application.Tests.Validators.Common.Language;

public class UpdateLanguageCommandValidatorTests
{
    private readonly UpdateLanguageCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldHaveError_WhenIdIsEmpty()
    {
        var command = new UpdateLanguageCommand(Guid.Empty, "en", "English");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("Language ID is required.");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenCodeIsEmpty()
    {
        var command = new UpdateLanguageCommand(Guid.NewGuid(), "", "English");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Code)
            .WithErrorMessage("Code is required.");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenCodeIsTooLong()
    {
        var command = new UpdateLanguageCommand(Guid.NewGuid(), "eng", "English");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Code)
            .WithErrorMessage("Code must be 2 characters or fewer.");
    }

    [Fact]
    public void Validate_ShouldHaveError_WhenNameIsEmpty()
    {
        var command = new UpdateLanguageCommand(Guid.NewGuid(), "en", "");

        var result = _validator.TestValidate(command);

        result.ShouldHaveValidationErrorFor(c => c.Name)
            .WithErrorMessage("Name is required.");
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenAllFieldsAreValid()
    {
        var command = new UpdateLanguageCommand(Guid.NewGuid(), "en", "English");

        var result = _validator.TestValidate(command);

        result.ShouldNotHaveValidationErrorFor(c => c.Id);
        result.ShouldNotHaveValidationErrorFor(c => c.Code);
        result.ShouldNotHaveValidationErrorFor(c => c.Name);
    }
}