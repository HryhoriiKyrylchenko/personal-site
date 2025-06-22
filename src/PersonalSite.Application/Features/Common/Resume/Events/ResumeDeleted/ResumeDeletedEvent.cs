namespace PersonalSite.Application.Features.Common.Resume.Events.ResumeDeleted;

public record ResumeDeletedEvent(string FileUrl) : INotification;
