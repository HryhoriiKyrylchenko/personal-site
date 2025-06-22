using PersonalSite.Infrastructure.Storage;

namespace PersonalSite.Application.Features.Common.Resume.Events.ResumeDeleted;

public class ResumeDeletedEventHandler : INotificationHandler<ResumeDeletedEvent>
{
    private readonly IStorageService _storageService;
    private readonly ILogger<ResumeDeletedEventHandler> _logger;

    public ResumeDeletedEventHandler(
        IStorageService storageService,
        ILogger<ResumeDeletedEventHandler> logger)
    {
        _storageService = storageService;
        _logger = logger;
    }

    public async Task Handle(ResumeDeletedEvent notification, CancellationToken cancellationToken)
    {
        try
        {
            await _storageService.DeleteFileAsync(notification.FileUrl, cancellationToken);
            _logger.LogInformation($"Deleted resume file {notification.FileUrl} from S3.");           
        }
        catch (Exception e)
        {
            _logger.LogError(e, $"Error deleting resume file {notification.FileUrl} from S3.");
        }
    }
}
