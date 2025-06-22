using PersonalSite.Domain.Entities.Projects;

namespace PersonalSite.Domain.Interfaces.Repositories.Projects;

public interface IProjectRepository : IRepository<Project>
{
    Task<List<Project>> GetAllWithFullDataAsync(CancellationToken cancellationToken = default);
    Task<Project?> GetLastAsync(CancellationToken cancellationToken);
    Task<Project?> GetWithFullDataAsync(Guid id, CancellationToken cancellationToken);
    Task<bool> IsSlugAvailableAsync(string requestSlug, CancellationToken cancellationToken);
}