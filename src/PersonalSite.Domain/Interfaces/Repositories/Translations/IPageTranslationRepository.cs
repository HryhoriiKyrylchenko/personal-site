namespace PersonalSite.Domain.Interfaces.Repositories.Translations;

public interface IPageTranslationRepository : IRepository<PageTranslation>
{
    Task<List<PageTranslation>> GetAllByPageKeyAsync(string pageKey, CancellationToken cancellationToken = default);
    Task<PageTranslation?> GetWithLanguageByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<IEnumerable<PageTranslation>> ListWithLanguageAsync(CancellationToken cancellationToken);
}