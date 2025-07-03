using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Common.Language.Queries.GetLanguageById;

namespace PersonalSite.Application.Tests.Validators.Common.Language;

public class GetLanguageByIdQueryValidatorTests
{
    private readonly GetLanguageByIdQueryValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_IdIsEmpty()
    {
        // Arrange
        var query = new GetLanguageByIdQuery(Guid.Empty);

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldHaveValidationErrorFor(q => q.Id)
            .WithErrorMessage("Language ID is required.");
    }

    [Fact]
    public void Should_NotHaveError_When_IdIsNotEmpty()
    {
        // Arrange
        var query = new GetLanguageByIdQuery(Guid.NewGuid());

        // Act
        var result = _validator.TestValidate(query);

        // Assert
        result.ShouldNotHaveValidationErrorFor(q => q.Id);
    }
}
