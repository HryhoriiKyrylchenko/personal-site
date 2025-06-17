namespace PersonalSite.Infrastructure.BackgroundProcessing;

public interface IBackgroundQueue
{
    void Enqueue(Func<CancellationToken, Task> workItem);
    Task<Func<CancellationToken, Task>> DequeueAsync(CancellationToken cancellationToken);
}