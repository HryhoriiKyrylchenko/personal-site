namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.DeleteAnalyticsEvent;

public class DeleteAnalyticsEventCommandValidator : AbstractValidator<DeleteAnalyticsEventCommand>
{
    public DeleteAnalyticsEventCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage("Event is required.");
    }
}