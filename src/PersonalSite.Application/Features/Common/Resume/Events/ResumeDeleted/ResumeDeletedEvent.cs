namespace PersonalSite.Application.Features.Common.Resume.Events.ResumeDeleted;

public class ResumeDeletedEvent : INotification
{
    public ResumeDeletedEvent(string fileUrl)
    {
        FileUrl = fileUrl;
    }

    public string FileUrl { get; }
}