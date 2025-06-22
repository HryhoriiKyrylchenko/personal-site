using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Common.Resume.Commands.UpdateResume;

public record UpdateResumeCommand(Guid Id, string FileUrl, string FileName, bool IsActive)
    : IRequest<Result>;