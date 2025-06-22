using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.DeleteAnalyticsEvent;

public record DeleteAnalyticsEventCommand(Guid Id) : IRequest<Result>;