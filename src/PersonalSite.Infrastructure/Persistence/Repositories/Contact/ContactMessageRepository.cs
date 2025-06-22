using PersonalSite.Domain.Common.Results;
using PersonalSite.Domain.Entities.Contact;
using PersonalSite.Domain.Interfaces.Repositories.Contact;

namespace PersonalSite.Infrastructure.Persistence.Repositories.Contact;

public class ContactMessageRepository : EfRepository<ContactMessage>, IContactMessageRepository
{
    public ContactMessageRepository(
        ApplicationDbContext context, 
        ILogger<ContactMessageRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<List<ContactMessage>> GetByIdsAsync(List<Guid> requestIds, CancellationToken cancellationToken)
    {
        if (requestIds.Count == 0)
            return new ();
        
        return await DbContext.ContactMessages
            .Where(cm => requestIds.Contains(cm.Id))
            .ToListAsync(cancellationToken);   
    }

    public async Task<PaginatedResult<ContactMessage>> GetFilteredAsync(int page, int pageSize, bool? isReadFilter, CancellationToken cancellationToken = default)
    {
        var query = DbContext.ContactMessages.AsQueryable().AsNoTracking();

        if (isReadFilter.HasValue)
            query = query.Where(x => x.IsRead == isReadFilter.Value);

        var total = await query.CountAsync(cancellationToken);

        var entities = await query
            .OrderByDescending(x => x.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
        
        return PaginatedResult<ContactMessage>.Success(entities, page, pageSize, total);   
    }
}