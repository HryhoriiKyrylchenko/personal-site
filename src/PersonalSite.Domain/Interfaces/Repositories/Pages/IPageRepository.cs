namespace PersonalSite.Domain.Interfaces.Repositories.Pages;

public interface IPageRepository : IRepository<Page>
{
    Task<Page?> GetByKeyAsync(string key, CancellationToken cancellationToken = default);
    Task<List<Page>> GetAllWithTranslationsAsync(CancellationToken cancellationToken = default);
    Task<Page?> GetWithTranslationByIdAsync(Guid id, CancellationToken cancellationToken);
}