using System.Collections.Concurrent;
using PersonalSite.Application.Features.Analytics.AnalyticsEvent.Commands.TrackAnalyticsEvent;
using PersonalSite.Infrastructure.BackgroundProcessing.BackgroundQueue;

namespace PersonalSite.Application.Features.Analytics.AnalyticsEvent.Events.PageViewedEvent;

public class PageViewedEventHandler : INotificationHandler<PageViewedEvent>
{
    private readonly IBackgroundQueue _queue;
    private readonly IServiceScopeFactory _scopeFactory;
    
    private static readonly ConcurrentDictionary<string, DateTime> _recentEvents = new();

    public PageViewedEventHandler(
        IBackgroundQueue queue, 
        IServiceScopeFactory scopeFactory)
    {
        _queue = queue;
        _scopeFactory = scopeFactory;
    }

    public Task Handle(PageViewedEvent notification, CancellationToken cancellationToken)
    {
        var key = $"{notification.PageSlug}:{notification.UserAgent}:{notification.Referrer}";

        if (_recentEvents.TryGetValue(key, out var lastTime) &&
            (DateTime.UtcNow - lastTime).TotalSeconds < 5)
            return Task.CompletedTask;

        _recentEvents[key] = DateTime.UtcNow;
        
        _queue.Enqueue(async ct =>
        {
            using var scope = _scopeFactory.CreateScope(); 
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            
            var command = new TrackAnalyticsEventCommand(
                EventType: "page_view",
                PageSlug: notification.PageSlug,
                Referrer: notification.Referrer,
                UserAgent: notification.UserAgent,
                AdditionalDataJson: "{}"
            );

            await mediator.Send(command, ct);
        });

        return Task.CompletedTask;
    }
}