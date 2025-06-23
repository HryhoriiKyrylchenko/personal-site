using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.DeleteAnalyticsEvent;

namespace PersonalSite.Application.Tests.Validators.Analytics.AnalyticsEvent;

public class DeleteAnalyticsEventCommandValidatorTests
{
    private readonly DeleteAnalyticsEventCommandValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_IdIsEmpty()
    {
        var command = new DeleteAnalyticsEventCommand(Guid.Empty);
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Id)
            .WithErrorMessage("Event is required.");
    }

    [Fact]
    public void Should_NotHaveError_When_IdIsValid()
    {
        var command = new DeleteAnalyticsEventCommand(Guid.NewGuid());
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}