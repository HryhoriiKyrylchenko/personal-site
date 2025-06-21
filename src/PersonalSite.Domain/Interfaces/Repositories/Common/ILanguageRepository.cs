namespace PersonalSite.Domain.Interfaces.Repositories.Common;

public interface ILanguageRepository : IRepository<Language>
{
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<Language?> GetByCodeAsync(string dtoLanguageCode, CancellationToken cancellationToken);
}