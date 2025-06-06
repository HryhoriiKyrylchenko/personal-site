namespace PersonalSite.Domain.Interfaces.Repositories.Contact;

public interface IContactMessageRepository : IRepository<ContactMessage>
{
    Task<List<ContactMessage>> GetUnreadMessagesAsync(CancellationToken cancellationToken = default);
    Task MarkAsReadAsync(Guid id, CancellationToken cancellationToken = default);
}