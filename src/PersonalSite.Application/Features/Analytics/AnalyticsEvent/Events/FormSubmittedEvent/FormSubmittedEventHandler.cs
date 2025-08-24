using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.TrackAnalyticsEvent;
using PersonalSite.Infrastructure.BackgroundProcessing.BackgroundQueue;

namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Events.FormSubmittedEvent;

public class FormSubmittedEventHandler : INotificationHandler<FormSubmittedEvent>
{
    private readonly IBackgroundQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;

    public FormSubmittedEventHandler(IBackgroundQueue queue, IServiceScopeFactory scopeFactory)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
    }

    public Task Handle(FormSubmittedEvent notification, CancellationToken cancellationToken)
    {
        _queue.Enqueue(async ct =>
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var command = new TrackAnalyticsEventCommand(
                EventType: "form_submitted",
                PageSlug: notification.PageSlug,
                Referrer: notification.Referrer,
                UserAgent: notification.UserAgent,
                AdditionalDataJson: notification.AdditionalDataJson
            );

            await mediator.Send(command, ct);
        });

        return Task.CompletedTask;
    }
}