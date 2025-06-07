namespace PersonalSite.Domain.Interfaces.Repositories.Translations;

public interface IPageTranslationRepository : IRepository<PageTranslation>
{
    Task<List<PageTranslation>> GetAllByPageKeyAsync(string pageKey, CancellationToken cancellationToken = default);
}