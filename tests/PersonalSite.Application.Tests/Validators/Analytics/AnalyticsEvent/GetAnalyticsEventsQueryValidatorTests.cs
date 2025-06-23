using FluentValidation.TestHelper;
using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Queries.GetAnalyticsEvents;

namespace PersonalSite.Application.Tests.Validators.Analytics.AnalyticsEvent;

public class GetAnalyticsEventsQueryValidatorTests
{
    private readonly GetAnalyticsEventsQueryValidator _validator = new();

    [Fact]
    public void Should_HaveError_When_PageIsLessThanOne()
    {
        var model = new GetAnalyticsEventsQuery(Page: 0);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.Page);
    }

    [Fact]
    public void Should_HaveError_When_PageSizeIsLessThanOne()
    {
        var model = new GetAnalyticsEventsQuery(PageSize: 0);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void Should_HaveError_When_PageSizeIsGreaterThan100()
    {
        var model = new GetAnalyticsEventsQuery(PageSize: 101);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PageSize);
    }

    [Fact]
    public void Should_HaveError_When_EventTypeIsTooLong()
    {
        var longEventType = new string('a', 101);
        var model = new GetAnalyticsEventsQuery(EventType: longEventType);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.EventType);
    }

    [Fact]
    public void Should_HaveError_When_PageSlugIsTooLong()
    {
        var longSlug = new string('a', 201);
        var model = new GetAnalyticsEventsQuery(PageSlug: longSlug);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.PageSlug);
    }

    [Fact]
    public void Should_HaveError_When_ToIsBeforeFrom()
    {
        var from = DateTime.UtcNow;
        var to = from.AddMinutes(-1);
        var model = new GetAnalyticsEventsQuery(From: from, To: to);
        var result = _validator.TestValidate(model);
        result.ShouldHaveValidationErrorFor(x => x.To);
    }

    [Fact]
    public void Should_NotHaveAnyErrors_When_AllValuesAreValid()
    {
        var model = new GetAnalyticsEventsQuery(
            Page: 1,
            PageSize: 20,
            EventType: "Click",
            PageSlug: "/home",
            From: DateTime.UtcNow.AddDays(-1),
            To: DateTime.UtcNow
        );

        var result = _validator.TestValidate(model);
        result.ShouldNotHaveAnyValidationErrors();
    }
}