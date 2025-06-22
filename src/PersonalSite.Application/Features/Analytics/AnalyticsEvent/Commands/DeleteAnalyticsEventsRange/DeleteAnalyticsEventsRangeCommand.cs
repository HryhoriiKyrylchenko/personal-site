using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.DeleteAnalyticsEventsRange;

public record DeleteAnalyticsEventsRangeCommand(List<Guid> Ids) : IRequest<Result>;