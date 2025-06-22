namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.DeleteAnalyticsEventsRange;

public class DeleteAnalyticsEventsRangeCommandValidator : AbstractValidator<DeleteAnalyticsEventsRangeCommand>
{
    public DeleteAnalyticsEventsRangeCommandValidator()
    {
        RuleFor(x => x.Ids)
            .NotEmpty().WithMessage("Ids are required.");
    }
}