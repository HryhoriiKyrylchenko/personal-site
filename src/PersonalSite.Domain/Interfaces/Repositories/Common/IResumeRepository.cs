namespace PersonalSite.Domain.Interfaces.Repositories.Common;

public interface IResumeRepository : IRepository<Resume>
{
    Task<Resume?> GetLastActiveAsync(CancellationToken cancellationToken = default);
}