namespace PersonalSite.Infrastructure.EventBus;

public class InMemoryEventBus : IEventPublisher
{
    private readonly IServiceProvider _serviceProvider;
    private readonly int _maxRetries = 3;
    private readonly TimeSpan _delayBetweenRetries = TimeSpan.FromSeconds(2);
    private readonly ILogger<InMemoryEventBus> _logger;

    public InMemoryEventBus(IServiceProvider serviceProvider,
        ILogger<InMemoryEventBus> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default) where TEvent : IEvent
    {
        var handlers = _serviceProvider.GetServices<IEventHandler<TEvent>>();

        foreach (var handler in handlers)
        {
            var attempt = 0;
            while (true)
            {
                try
                {
                    await handler.HandleAsync(@event, cancellationToken);
                    break;
                }
                catch (Exception ex)
                {
                    attempt++;
                    if (attempt >= _maxRetries)
                    {
                        _logger.LogWarning($"Handler {handler.GetType().Name} failed after {attempt} attempts: {ex.Message}");
                        break;
                    }

                    _logger.LogWarning($"Handler {handler.GetType().Name} failed attempt {attempt}. Retrying in {_delayBetweenRetries.TotalSeconds}s...");
                    await Task.Delay(_delayBetweenRetries, cancellationToken);
                }
            }
        }
    }
}
