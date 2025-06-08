namespace PersonalSite.Infrastructure.Persistence.Repositories.Contact;

public class ContactMessageRepository : EfRepository<ContactMessage>, IContactMessageRepository
{
    public ContactMessageRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<List<ContactMessage>> GetUnreadMessagesAsync(CancellationToken cancellationToken = default)
    {
        return await DbContext.ContactMessages
            .Where(cm => !cm.IsRead)
            .ToListAsync(cancellationToken);
    }

    public async Task MarkAsReadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var message = await DbContext.ContactMessages.FindAsync([id], cancellationToken);
        if (message is { IsRead: false })
        {
            message.IsRead = true;
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }

    public async Task MarkAsUnreadAsync(Guid id, CancellationToken cancellationToken)
    {
        var message = await DbContext.ContactMessages.FindAsync([id], cancellationToken);
        if (message is { IsRead: true })
        {
            message.IsRead = false;
            await DbContext.SaveChangesAsync(cancellationToken);
        }
    }
}