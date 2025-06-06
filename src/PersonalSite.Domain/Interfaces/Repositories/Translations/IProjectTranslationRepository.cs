namespace PersonalSite.Domain.Interfaces.Repositories.Translations;

public interface IProjectTranslationRepository : IRepository<ProjectTranslation>
{
    Task<ProjectTranslation?> GetByProjectIdAndLanguageAsync(Guid projectId, string languageCode, CancellationToken cancellationToken = default);
}