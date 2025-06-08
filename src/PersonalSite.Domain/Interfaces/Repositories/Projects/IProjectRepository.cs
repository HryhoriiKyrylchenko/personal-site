namespace PersonalSite.Domain.Interfaces.Repositories.Projects;

public interface IProjectRepository : IRepository<Project>
{
    Task<Project?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<List<Project>> GetAllWithFullDataAsync(CancellationToken cancellationToken = default);
    Task<Project?> GetLastAsync(CancellationToken cancellationToken);
    Task<Project?> GetWithFullDataAsync(Guid id, CancellationToken cancellationToken);
}