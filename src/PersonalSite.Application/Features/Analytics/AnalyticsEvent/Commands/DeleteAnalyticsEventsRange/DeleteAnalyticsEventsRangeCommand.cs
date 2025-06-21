namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.DeleteAnalyticsEventsRange;

public record DeleteAnalyticsEventsRangeCommand(List<Guid> Ids) : IRequest<Result>;