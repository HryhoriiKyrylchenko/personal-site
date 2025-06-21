namespace PersonalSite.Application.Features.Common.Resume.Events.ResumeDeleted;

public class ResumeDeletedEventHandler : INotificationHandler<ResumeDeletedEvent>
{
    private readonly ILogger<ResumeDeletedEventHandler> _logger;

    public ResumeDeletedEventHandler(ILogger<ResumeDeletedEventHandler> logger)
    {
        _logger = logger;
    }

    public Task Handle(ResumeDeletedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            // TODO: Use your future IStorageService to delete from S3
            _logger.LogInformation("Resume file {FileUrl} would be deleted from S3 (mock).", notification.FileUrl);

            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error deleting resume file from S3.");
            return Task.CompletedTask;      
        }
    }
}
