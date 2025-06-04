namespace PersonalSite.Domain.Repositories.Translations;

public interface ILanguageRepository : IRepository<Language>
{
    Task<Language?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}