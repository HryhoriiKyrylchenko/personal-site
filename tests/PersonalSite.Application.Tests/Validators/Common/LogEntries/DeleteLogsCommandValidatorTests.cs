using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Common.LogEntries.Commands.DeleteLogs;

namespace PersonalSite.Application.Tests.Validators.Common.LogEntries;

public class DeleteLogsCommandValidatorTests
{
    private readonly DeleteLogsCommandValidator _validator;

    public DeleteLogsCommandValidatorTests()
    {
        _validator = new DeleteLogsCommandValidator();
    }

    [Fact]
    public void Should_Have_Error_When_Ids_Is_Empty()
    {
        // Arrange
        var command = new DeleteLogsCommand(new List<Guid>());

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldHaveValidationErrorFor(c => c.Ids)
            .WithErrorMessage("At least one log ID must be provided.");
    }

    [Fact]
    public void Should_Not_Have_Error_When_Ids_Contain_Elements()
    {
        // Arrange
        var command = new DeleteLogsCommand(new List<Guid> { Guid.NewGuid() });

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveValidationErrorFor(c => c.Ids);
    }
}