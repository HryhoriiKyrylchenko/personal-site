namespace PersonalSite.Infrastructure.BackgroundProcessing;

public class BackgroundQueue : IBackgroundQueue
{
    private readonly Channel<Func<CancellationToken, Task>> _queue;

    public BackgroundQueue()
    {
        _queue = Channel.CreateUnbounded<Func<CancellationToken, Task>>();
    }

    public void Enqueue(Func<CancellationToken, Task> workItem)
    {
        ArgumentNullException.ThrowIfNull(workItem);

        _queue.Writer.TryWrite(workItem);
    }

    public void Schedule(Func<CancellationToken, Task> work, TimeSpan delay)
    {
        if (work == null) throw new ArgumentNullException(nameof(work));

        Func<CancellationToken, Task> delayedWork = async token =>
        {
            await Task.Delay(delay, token);
            await work(token);
        };

        _queue.Writer.TryWrite(delayedWork);
    }

    public async Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken)
    {
        var workItem = await _queue.Reader.ReadAsync(cancellationToken);
        return workItem;
    }
}