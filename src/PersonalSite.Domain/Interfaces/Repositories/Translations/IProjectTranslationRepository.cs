using PersonalSite.Domain.Entities.Translations;

namespace PersonalSite.Domain.Interfaces.Repositories.Translations;

public interface IProjectTranslationRepository : IRepository<ProjectTranslation>
{
    Task<List<ProjectTranslation>> GetByProjectIdAsync(Guid projectId, CancellationToken cancellationToken = default);
}