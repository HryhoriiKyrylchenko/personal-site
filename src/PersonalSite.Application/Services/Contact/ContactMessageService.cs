namespace PersonalSite.Application.Services.Contact;

public class ContactMessageService : 
    CrudServiceBase<ContactMessage, ContactMessageDto, ContactMessageAddRequest, ContactMessageUpdateRequest>, 
    IContactMessageService
{
    private IContactMessageRepository _contactMessageRepository;
    
    public ContactMessageService(
        IContactMessageRepository repository, 
        IUnitOfWork unitOfWork) 
        : base(repository, unitOfWork)
    {
        _contactMessageRepository = repository;
    }
    
    public override async Task<ContactMessageDto?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var entity = await Repository.GetByIdAsync(id, cancellationToken);
        return entity == null
            ? null
            : EntityToDtoMapper.MapContactMessageToDto(entity);
    }

    public override async Task<IReadOnlyList<ContactMessageDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var entities = await Repository.ListAsync(cancellationToken);
        return EntityToDtoMapper.MapContactMessagesToDtoList(entities);
    }

    public override async Task AddAsync(ContactMessageAddRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task UpdateAsync(ContactMessageUpdateRequest request, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public override async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public async Task<List<ContactMessageDto>> GetUnreadMessagesAsync(CancellationToken cancellationToken = default)
    {
        var unreadMessages = await _contactMessageRepository.GetUnreadMessagesAsync(cancellationToken);
        return EntityToDtoMapper.MapContactMessagesToDtoList(unreadMessages);
    }

    public async Task MarkMessageAsReadAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await _contactMessageRepository.MarkAsReadAsync(id, cancellationToken);
    }
}