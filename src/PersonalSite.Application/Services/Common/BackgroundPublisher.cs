namespace PersonalSite.Application.Services.Common;

public class BackgroundPublisher : IBackgroundPublisher
{
    private readonly IBackgroundQueue _queue;
    private readonly IServiceProvider _serviceProvider;

    public BackgroundPublisher(IBackgroundQueue queue, IServiceProvider serviceProvider)
    {
        _queue = queue;
        _serviceProvider = serviceProvider;
    }

    public void Schedule<T>(T command, DateTime executeAtUtc)
        where T : class
    {
        var delay = executeAtUtc - DateTime.UtcNow;
        if (delay < TimeSpan.Zero)
            delay = TimeSpan.Zero;

        var serializedCommand = JsonSerializer.Serialize(command);
        var typeName = typeof(T).AssemblyQualifiedName!;

        _queue.Schedule(async token =>
        {
            using var scope = _serviceProvider.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var type = Type.GetType(typeName);
            if (type == null) return;

            var deserialized = JsonSerializer.Deserialize(serializedCommand, type);
            if (deserialized is not null)
                await mediator.Send(deserialized, token);
        }, delay);
    }
}
