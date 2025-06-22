using PersonalSite.Domain.Common.Results;

namespace PersonalSite.Application.Features.Common.Resume.Commands.CreateResume;

public record CreateResumeCommand(string FileUrl, string FileName, bool IsActive)
    : IRequest<Result<Guid>>;