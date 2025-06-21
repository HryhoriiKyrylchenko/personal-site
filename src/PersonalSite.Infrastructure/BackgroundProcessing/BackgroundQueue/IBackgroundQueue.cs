namespace PersonalSite.Infrastructure.BackgroundProcessing.BackgroundQueue;

public interface IBackgroundQueue
{
    void Enqueue(Func<CancellationToken, Task> workItem);
    void Schedule(Func<CancellationToken, Task> work, TimeSpan delay);
    Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
}