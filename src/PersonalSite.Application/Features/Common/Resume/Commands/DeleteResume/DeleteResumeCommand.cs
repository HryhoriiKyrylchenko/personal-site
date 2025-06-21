namespace PersonalSite.Application.Features.Common.Resume.Commands.DeleteResume;

public record DeleteResumeCommand(Guid Id) : IRequest<Result>;