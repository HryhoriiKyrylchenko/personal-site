namespace PersonalSite.Infrastructure.Persistence.Repositories.Contact;

public class ContactMessageRepository : EfRepository<ContactMessage>, IContactMessageRepository
{
    public ContactMessageRepository(
        ApplicationDbContext context, 
        ILogger<ContactMessageRepository> logger,
        IServiceProvider serviceProvider) 
        : base(context, logger, serviceProvider) { }

    public async Task<List<ContactMessage>> GetUnreadMessagesAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.ContactMessages
            .Where(cm => !cm.IsRead)
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAsReadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        
        var message = await DbContext.ContactMessages.FindAsync([id], cancellationToken);
        if (message is { IsRead: false })
        {
            message.IsRead = true;
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task MarkAsUnreadAsync(Guid id, CancellationToken cancellationToken)
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Id cannot be empty", nameof(id));
        
        var message = await DbContext.ContactMessages.FindAsync([id], cancellationToken);
        if (message is { IsRead: true })
        {
            message.IsRead = false;
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}