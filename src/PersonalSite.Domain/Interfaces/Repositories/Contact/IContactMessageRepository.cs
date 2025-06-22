using PersonalSite.Domain.Entities.Contact;

namespace PersonalSite.Domain.Interfaces.Repositories.Contact;

public interface IContactMessageRepository : IRepository<ContactMessage>
{
    Task<List<ContactMessage>> GetByIdsAsync(List<Guid> requestIds, CancellationToken cancellationToken);
    Task<PaginatedResult<ContactMessage>> GetFilteredAsync(
        int page,
        int pageSize,
        bool? isReadFilter,
        CancellationToken cancellationToken = default);
}