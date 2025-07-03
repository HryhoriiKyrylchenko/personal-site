using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.TrackAnalyticsEvent;

namespace PersonalSite.Application.Tests.Validators.Analytics.AnalyticsEvent;

public class TrackAnalyticsEventCommandValidatorTests
{
    private readonly TrackAnalyticsEventCommandValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_EventTypeIsEmpty()
    {
        var command = new TrackAnalyticsEventCommand(
            EventType: "",
            PageSlug: "valid-slug",
            Referrer: null,
            UserAgent: null,
            AdditionalDataJson: null);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.EventType)
              .WithErrorMessage("Event type is required.");
    }

    [Fact]
    public void Should_HaveError_When_EventTypeTooLong()
    {
        var command = new TrackAnalyticsEventCommand(
            EventType: new string('a', 101),
            PageSlug: "valid-slug",
            Referrer: null,
            UserAgent: null,
            AdditionalDataJson: null);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.EventType)
              .WithErrorMessage("Event type must be 100 characters or fewer.");
    }

    [Fact]
    public void Should_HaveError_When_PageSlugIsEmpty()
    {
        var command = new TrackAnalyticsEventCommand(
            EventType: "event",
            PageSlug: "",
            Referrer: null,
            UserAgent: null,
            AdditionalDataJson: null);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PageSlug)
              .WithErrorMessage("Page slug is required.");
    }

    [Fact]
    public void Should_HaveError_When_PageSlugTooLong()
    {
        var command = new TrackAnalyticsEventCommand(
            EventType: "event",
            PageSlug: new string('a', 201),
            Referrer: null,
            UserAgent: null,
            AdditionalDataJson: null);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.PageSlug)
              .WithErrorMessage("Page slug must be 200 characters or fewer.");
    }

    [Fact]
    public void Should_HaveError_When_ReferrerTooLong()
    {
        var command = new TrackAnalyticsEventCommand(
            EventType: "event",
            PageSlug: "slug",
            Referrer: new string('a', 513),
            UserAgent: null,
            AdditionalDataJson: null);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.Referrer);
    }

    [Fact]
    public void Should_HaveError_When_UserAgentTooLong()
    {
        var command = new TrackAnalyticsEventCommand(
            EventType: "event",
            PageSlug: "slug",
            Referrer: null,
            UserAgent: new string('a', 513),
            AdditionalDataJson: null);

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.UserAgent);
    }

    [Fact]
    public void Should_HaveError_When_AdditionalDataJsonIsInvalid()
    {
        var command = new TrackAnalyticsEventCommand(
            EventType: "event",
            PageSlug: "slug",
            Referrer: null,
            UserAgent: null,
            AdditionalDataJson: "{ invalid json }");

        var result = _validator.TestValidate(command);
        result.ShouldHaveValidationErrorFor(x => x.AdditionalDataJson)
              .WithErrorMessage("AdditionalDataJson must be valid JSON.");
    }

    [Fact]
    public void Should_NotHaveError_When_AllFieldsValid()
    {
        var validJson = "{\"key\":\"value\"}";
        var command = new TrackAnalyticsEventCommand(
            EventType: "event",
            PageSlug: "slug",
            Referrer: "http://referrer.com",
            UserAgent: "UserAgent",
            AdditionalDataJson: validJson);

        var result = _validator.TestValidate(command);
        result.ShouldNotHaveAnyValidationErrors();
    }
}