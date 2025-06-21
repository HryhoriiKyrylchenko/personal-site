namespace PersonalSite.Application.Features.Projects.Project.Commands.DeleteProject;

public record DeleteProjectCommand(Guid Id) : IRequest<Result>;