namespace PersonalSite.Application.Services.Projects;

public interface IProjectService : ICrudService<ProjectDto, ProjectAddRequest, ProjectUpdateRequest>
{
    Task<ProjectDto?> GetProjectAsync(Guid id, CancellationToken cancellationToken = default);
    Task<ProjectDto?> GetLastProjectAsync(CancellationToken cancellationToken = default);
    Task<List<ProjectDto>> GetProjectsFullDataAsync(CancellationToken cancellationToken = default);
}