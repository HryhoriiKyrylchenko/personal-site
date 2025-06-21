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
}