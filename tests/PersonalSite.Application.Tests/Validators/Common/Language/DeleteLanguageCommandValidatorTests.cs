using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Common.Language.Commands.DeleteLanguage;

namespace PersonalSite.Application.Tests.Validators.Common.Language;

public class DeleteLanguageCommandValidatorTests
{
    private readonly DeleteLanguageCommandValidator _validator = new();

    [Fact]
    public void Validate_ShouldHaveError_WhenIdIsEmpty()
    {
        // Arrange
        var command = new DeleteLanguageCommand(Guid.Empty);

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Id)
            .WithErrorMessage("Language ID is required.");
    }

    [Fact]
    public void Validate_ShouldNotHaveError_WhenIdIsNotEmpty()
    {
        // Arrange
        var command = new DeleteLanguageCommand(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Id);
    }
}