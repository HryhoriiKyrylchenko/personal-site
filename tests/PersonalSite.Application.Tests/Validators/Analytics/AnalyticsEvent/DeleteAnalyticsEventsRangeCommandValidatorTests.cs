using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.DeleteAnalyticsEventsRange;

namespace PersonalSite.Application.Tests.Validators.Analytics.AnalyticsEvent;

public class DeleteAnalyticsEventsRangeCommandValidatorTests
{
    private readonly DeleteAnalyticsEventsRangeCommandValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_IdsIsEmpty()
    {
        var command = new DeleteAnalyticsEventsRangeCommand(new List<Guid>());
        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Ids)
            .WithErrorMessage("Ids are required.");
    }

    [Fact]
    public void Should_NotHaveError_When_IdsAreProvided()
    {
        var command = new DeleteAnalyticsEventsRangeCommand(new List<Guid> { Guid.NewGuid() });
        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}