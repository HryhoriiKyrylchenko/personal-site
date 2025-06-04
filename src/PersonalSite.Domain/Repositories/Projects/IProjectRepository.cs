namespace PersonalSite.Domain.Repositories.Projects;

public interface IProjectRepository : IRepository<Project>
{
    Task<Project?> GetBySlugAsync(string slug, CancellationToken cancellationToken = default);
    Task<List<Project>> GetAllWithTranslationsAsync(string languageCode, CancellationToken cancellationToken = default);
}