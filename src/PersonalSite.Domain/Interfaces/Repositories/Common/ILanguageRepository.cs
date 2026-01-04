using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Domain.Interfaces.Repositories.Common;

public interface ILanguageRepository : IRepository<Language>
{
    Task<bool> ExistsByCodeAsync(string code, CancellationToken cancellationToken = default);
    Task<Language?> GetByCodeAsync(string dtoLanguageCode, CancellationToken cancellationToken);
    Task<List<Language>> GetAllActiveAsync(CancellationToken cancellationToken = default);
}