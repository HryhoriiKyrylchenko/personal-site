namespace PersonalSite.Domain.Interfaces.Repositories.Translations;

public interface IPageTranslationRepository : IRepository<PageTranslation>
{
    Task<PageTranslation?> GetByPageKeyAndLanguageAsync(string pageKey, string languageCode, CancellationToken cancellationToken = default);
}