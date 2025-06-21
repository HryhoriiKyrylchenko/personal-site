namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Queries.GetAnalyticsEvents;

public class GetAnalyticsEventsQueryValidator : AbstractValidator<GetAnalyticsEventsQuery>
{
    public GetAnalyticsEventsQueryValidator()
    {
        RuleFor(x => x.Page)
            .GreaterThan(0).WithMessage("Page must be greater than 0.");

        RuleFor(x => x.PageSize)
            .InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");

        RuleFor(x => x.EventType)
            .MaximumLength(100).WithMessage("Event type must be 100 characters or fewer.")
            .When(x => !string.IsNullOrWhiteSpace(x.EventType));

        RuleFor(x => x.PageSlug)
            .MaximumLength(200).WithMessage("Page slug must be 200 characters or fewer.")
            .When(x => !string.IsNullOrWhiteSpace(x.PageSlug));

        RuleFor(x => x.To)
            .GreaterThanOrEqualTo(x => x.From!.Value)
            .WithMessage("'To' must be after or equal to 'From'.")
            .When(x => x.From.HasValue && x.To.HasValue);
    }
}