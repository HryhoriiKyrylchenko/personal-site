using PersonalSite.Application.Features.Projects.Common.Dtos;

namespace PersonalSite.Application.Services.Projects;

public interface IProjectService : ICrudService<ProjectDto, ProjectAddRequest, ProjectUpdateRequest>
{
    Task<ProjectDto?> GetLastProjectAsync(CancellationToken cancellationToken = default);
}