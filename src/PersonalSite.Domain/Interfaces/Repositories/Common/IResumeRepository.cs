using PersonalSite.Domain.Entities.Common;

namespace PersonalSite.Domain.Interfaces.Repositories.Common;

public interface IResumeRepository : IRepository<Resume>
{
    Task<Resume?> GetLastActiveAsync(CancellationToken cancellationToken = default);
    Task<PaginatedResult<Resume>> GetFilteredAsync(
        int page,
        int pageSize,
        bool? isActive,
        CancellationToken cancellationToken = default);
}