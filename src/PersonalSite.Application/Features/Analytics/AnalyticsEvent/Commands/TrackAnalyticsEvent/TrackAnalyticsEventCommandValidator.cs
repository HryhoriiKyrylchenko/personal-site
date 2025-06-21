namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.TrackAnalyticsEvent;

public class TrackAnalyticsEventCommandValidator : AbstractValidator<TrackAnalyticsEventCommand>
{
    public TrackAnalyticsEventCommandValidator()
    {
        RuleFor(x => x.EventType)
            .NotEmpty().WithMessage("Event type is required.")
            .MaximumLength(100).WithMessage("Event type must be 100 characters or fewer.");

        RuleFor(x => x.PageSlug)
            .NotEmpty().WithMessage("Page slug is required.")
            .MaximumLength(200).WithMessage("Page slug must be 200 characters or fewer.");

        RuleFor(x => x.Referrer)
            .MaximumLength(512).When(x => !string.IsNullOrWhiteSpace(x.Referrer));

        RuleFor(x => x.UserAgent)
            .MaximumLength(512).When(x => !string.IsNullOrWhiteSpace(x.UserAgent));

        RuleFor(x => x.AdditionalDataJson)
            .Must(BeValidJson).When(x => !string.IsNullOrWhiteSpace(x.AdditionalDataJson))
            .WithMessage("AdditionalDataJson must be valid JSON.");
    }

    private bool BeValidJson(string? json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return true;

        try
        {
            JsonDocument.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }
}