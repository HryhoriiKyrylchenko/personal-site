namespace PersonalSite.Domain.Interfaces.Repositories.Common;

public interface ILanguageRepository : IRepository<Language>
{
    Task<Language?> GetByCodeAsync(string code, CancellationToken cancellationToken = default);
}